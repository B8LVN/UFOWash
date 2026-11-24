using System;
using UFOWASH;
using System.IO;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

int passed = 0;
void Check(bool condition, string name)
{
    if (!condition) throw new Exception("FAIL: " + name);
    Console.WriteLine("PASS: " + name);
    passed++;
}
bool Near(float a, float b) => Math.Abs(a - b) < 0.0001f;

var panel = new UfoWashCoatingState(new[] { 1f, 3f }, 0.9f);
panel.Treat(0, 0, 100f);
Check(Near(panel.DirtAt(0), 0.5f) && Near(panel.DirtAt(1), 0.9f), "Pre-rinse is local and has a floor");
Check(Near(panel.Dirt, 0.8f), "Panel totals are area-weighted");
panel.Treat(0, 1, 1f);
Check(Near(panel.FoamAt(0), 1f) && Near(panel.FoamAt(1), 0f), "Foam coats only the contact cell");
Check(Near(panel.Foam, 0.25f), "One coated spot cannot satisfy the panel foam gate");
panel.Treat(0, 2, 0.5f);
Check(panel.DirtAt(0) < 0.5f && panel.FoamAt(0) < 1f && Near(panel.DirtAt(1), 0.9f),
    "Rinsing removes local dirt and foam without changing neighbors");
Check(Near(panel.FoamAt(1), 0f), "Rinsing never creates foam");
panel.Treat(0, 2, 100f);
panel.Treat(1, 2, 100f);
Check(Near(panel.Dirt, 0f) && Near(panel.Foam, 0f), "Final rinse cannot strand dirt after suds run out");
panel.Treat(0, 0, 1f);
Check(Near(panel.DirtAt(0), 0f), "Pre-rinse does not put dirt back on a clean cell");
panel.Reset(0.72f);
Check(Near(panel.Dirt, 0.72f) && Near(panel.Foam, 0f), "Next contract resets all coating cells");
panel.Treat(0, 1, 0.4f);
float persistent = panel.Foam;
panel.Treat(1, 0, 0.2f);
Check(Near(panel.Foam, persistent) && panel.Foam > 0f, "Foam persists while a different cell is worked");
panel.Treat(0, 0, float.NaN);
panel.Treat(0, 0, float.PositiveInfinity);
panel.Treat(0, 0, -1f);
Check(Near(panel.DirtAt(0), 0.72f), "Invalid time does not mutate coating state");
bool rejected = false;
try { _ = new UfoWashCoatingState(Array.Empty<float>(), 1f); } catch (ArgumentException) { rejected = true; }
Check(rejected, "Empty exposed geometry fails explicitly");
rejected = false;
try { _ = new UfoWashCoatingState(new[] { 0f }, 1f); } catch (ArgumentException) { rejected = true; }
Check(rejected, "Invalid cell areas fail explicitly");
var slow = new UfoWashCoatingState(new[] { 1f }, 0.9f);
var fast = new UfoWashCoatingState(new[] { 1f }, 0.9f);
slow.Treat(0, 0, 0.1f); fast.Treat(0, 0, 0.2f);
Check(fast.Dirt < slow.Dirt, "Increased effective tool power changes work rate");
// Reproduce the actual upper-shell profile and index order from the saucer builder.
var profile = new (float r, float y)[] { (0,1.42f),(.45f,1.38f),(.95f,1.22f),
    (1.35f,.94f),(1.62f,.62f),(1.7f,.42f),(0,.4f) };
var positions = new float[profile.Length * 64 * 3];
var indices = new System.Collections.Generic.List<int>();
for (int ring = 0; ring < profile.Length; ring++)
    for (int segment = 0; segment < 64; segment++)
    {
        int index = (ring * 64 + segment) * 3;
        double angle = segment / 64.0 * Math.PI * 2;
        positions[index] = (float)Math.Cos(angle) * profile[ring].r;
        positions[index + 1] = profile[ring].y;
        positions[index + 2] = (float)Math.Sin(angle) * profile[ring].r;
        if (ring == profile.Length - 1) continue;
        int a = ring * 64 + segment, b = ring * 64 + (segment + 1) % 64;
        int c = a + 64, d = b + 64;
        indices.AddRange(new[] { a,c,b,b,c,d });
    }
var legacyIndices = indices.ToArray();
Check(UfoWashHullWinding.IsInward(positions, legacyIndices), "Legacy upper saucer profile is inward-facing");
var repaired = (int[])legacyIndices.Clone();
for (int i = 0; i < repaired.Length; i += 3)
    (repaired[i + 1], repaired[i + 2]) = (repaired[i + 2], repaired[i + 1]);
Check(!UfoWashHullWinding.IsInward(positions, repaired), "Reversing upper-shell triangles makes winding outward");
Check(UfoWashHullWinding.IsInward(positions, legacyIndices), "Original hull indices remain unchanged");
Check(!UfoWashHullWinding.IsInward(new float[] {0,0,0, 0,0,1, 1,0,0}, new[] {0,1,2}),
    "Open flat QA panel is not reversed");
Console.WriteLine($"{passed} deterministic coating/geometry checks passed. Not Unity import, gameplay or visual acceptance.");
var parseOptions = new CSharpParseOptions(LanguageVersion.CSharp9, preprocessorSymbols: new[] { "UNITY_EDITOR" });
int parsed = 0;
foreach (string path in Directory.EnumerateFiles("Assets/UFO_WASH", "*.cs", SearchOption.AllDirectories))
{
    var syntax = CSharpSyntaxTree.ParseText(File.ReadAllText(path), parseOptions, path);
    foreach (var diagnostic in syntax.GetDiagnostics())
        if (diagnostic.Severity == DiagnosticSeverity.Error) throw new Exception(diagnostic.ToString());
    parsed++;
}
Console.WriteLine($"PASS: C# 9 syntax parsed for {parsed} game/editor files. No Unity API/type or shader compilation was performed.");
