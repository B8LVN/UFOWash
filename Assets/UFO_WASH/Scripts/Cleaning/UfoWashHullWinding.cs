namespace UFOWASH
{
    public static class UfoWashHullWinding
    {
        // Signed volume is translation-independent for a closed triangular hull.
        // Open/flat panels have approximately zero volume and are left unchanged.
        public static bool IsInward(float[] xyz, int[] triangles)
        {
            double volume = 0;
            for (int i = 0; i < triangles.Length; i += 3)
            {
                int a = triangles[i] * 3, b = triangles[i + 1] * 3, c = triangles[i + 2] * 3;
                volume += xyz[a] * ((double)xyz[b + 1] * xyz[c + 2] - (double)xyz[b + 2] * xyz[c + 1])
                    + xyz[a + 1] * ((double)xyz[b + 2] * xyz[c] - (double)xyz[b] * xyz[c + 2])
                    + xyz[a + 2] * ((double)xyz[b] * xyz[c + 1] - (double)xyz[b + 1] * xyz[c]);
            }
            return volume < -0.000001;
        }
    }
}
