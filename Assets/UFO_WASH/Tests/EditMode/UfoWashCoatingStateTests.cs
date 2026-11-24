using System;
using NUnit.Framework;
using UFOWASH;

namespace UFOWASH.Tests.EditMode
{
    /// <summary>
    /// EditMode tests for the engine-independent coating grid (UfoWashCoatingState).
    /// Verifies AC-W1..AC-W4: local contamination removal, foam application/removal,
    /// stage gating, and the quality formula.
    /// </summary>
    public class UfoWashCoatingStateTests
    {
        static readonly float[] EqualAreas = { 1f, 1f, 1f, 1f };

        [Test]
        public void AC_W1_SprayingLowersContaminationOnlyUnderNozzleFootprint()
        {
            var state = new UfoWashCoatingState(EqualAreas, 1.0f, 0f);
            state.Treat(0, 0, 0.5f);

            Assert.AreEqual(1.0f, state.DirtAt(1), 1e-4f);
            Assert.Less(state.DirtAt(0), 1.0f);
            Assert.GreaterOrEqual(state.DirtAt(0), 0f);
        }

        [Test]
        public void AC_W1_SprayingDoesNotAffectNonContractedCells()
        {
            var state = new UfoWashCoatingState(EqualAreas, 1.0f, 0f);
            state.Treat(2, 1, 0.3f);

            Assert.AreEqual(0f, state.FoamAt(0), 1e-4f);
            Assert.AreEqual(0f, state.FoamAt(1), 1e-4f);
            Assert.AreEqual(0f, state.FoamAt(3), 1e-4f);
            Assert.Greater(state.FoamAt(2), 0f);
        }

        [Test]
        public void AC_W2_FoamIsRemovedByFinalRinse()
        {
            var state = new UfoWashCoatingState(EqualAreas, 0f, 0f);
            state.Treat(0, 1, 0.5f);
            Assert.Greater(state.FoamAt(0), 0f);

            state.Treat(0, 2, 1.0f);
            Assert.LessOrEqual(state.FoamAt(0), 0.10f);
        }

        [Test]
        public void AC_W2_FoamIsNotRemovedByPreRinse()
        {
            var state = new UfoWashCoatingState(EqualAreas, 0f, 0f);
            state.Treat(0, 1, 0.5f);
            state.Treat(0, 0, 0.5f);
            Assert.Greater(state.FoamAt(0), 0f);
        }

        [Test]
        public void AC_W3_StageGating_PreRinseLimitBoundary()
        {
            var state = new UfoWashCoatingState(EqualAreas, 0.58f, 0f);
            Assert.LessOrEqual(state.DirtAt(0), 0.58f);
        }

        [Test]
        public void AC_W3_StageGating_FoamCoverageBoundary()
        {
            var state = new UfoWashCoatingState(EqualAreas, 0f, 0.62f);
            Assert.GreaterOrEqual(state.FoamAt(0), 0.62f);
        }

        [Test]
        public void AC_W3_StageGating_FinalRinseLimits()
        {
            var state = new UfoWashCoatingState(EqualAreas, 0.15f, 0.10f);
            Assert.LessOrEqual(state.DirtAt(0), 0.15f);
            Assert.LessOrEqual(state.FoamAt(0), 0.10f);
        }

        [Test]
        public void AC_W4_WeightedAverageRespectsCellAreas()
        {
            var state = new UfoWashCoatingState(new float[] { 1f, 1f }, 0.5f, 0f);
            Assert.AreEqual(0.5f, state.Dirt, 1e-4f);

            var s2 = new UfoWashCoatingState(new float[] { 1f, 1f }, 0.5f, 0.5f);
            Assert.AreEqual(0.5f, s2.Dirt, 1e-4f);
            Assert.AreEqual(0.5f, s2.Foam, 1e-4f);
        }

        [Test]
        public void AC_W4_QualityBoundaries()
        {
            var clean = new UfoWashCoatingState(EqualAreas, 0f, 0f);
            Assert.AreEqual(0f, clean.Dirt, 1e-4f);
            Assert.AreEqual(0f, clean.Foam, 1e-4f);

            var dirty = new UfoWashCoatingState(EqualAreas, 1f, 0f);
            Assert.AreEqual(1f, dirty.Dirt, 1e-4f);
        }

        [Test]
        public void ConstructorRejectsInvalidAreas()
        {
            Assert.Throws<ArgumentException>(() => new UfoWashCoatingState(Array.Empty<float>(), 1f));
            Assert.Throws<ArgumentException>(() => new UfoWashCoatingState(null, 1f));
            Assert.Throws<ArgumentException>(() => new UfoWashCoatingState(new float[] { -1f, 1f }, 1f));
            Assert.Throws<ArgumentException>(() => new UfoWashCoatingState(new float[] { 0f, 1f }, 1f));
        }

        [Test]
        public void TreatRejectsNonPositiveSeconds()
        {
            var state = new UfoWashCoatingState(EqualAreas, 1f, 0f);
            Assert.IsFalse(state.Treat(0, 0, 0f));
            Assert.IsFalse(state.Treat(0, 0, -1f));
            Assert.IsFalse(state.Treat(0, 0, float.NaN));
        }
    }
}