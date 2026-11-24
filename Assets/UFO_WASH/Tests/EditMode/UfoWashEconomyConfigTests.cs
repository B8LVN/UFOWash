using NUnit.Framework;
using UFOWASH;

namespace UFOWASH.Tests.EditMode
{
    /// <summary>
    /// EditMode tests for UfoWashEconomyConfig. Verifies AC-E1: tip and reputation
    /// tables match the GDD at every boundary (83.9, 84, 89.9, 90, 98.4, 98.5).
    /// </summary>
    public class UfoWashEconomyConfigTests
    {
        UfoWashEconomyConfig config;

        [SetUp]
        public void SetUp() => config = new UfoWashEconomyConfig();

        [Test]
        public void AC_E1_TipRateAtQualityBoundaries()
        {
            // Below 84% → no tip.
            Assert.AreEqual(0f, config.TipRateForQuality(0.839f), 1e-4f);
            // At 84% → 8% tip.
            Assert.AreEqual(0.08f, config.TipRateForQuality(0.84f), 1e-4f);
            // Below 90% → still 8%.
            Assert.AreEqual(0.08f, config.TipRateForQuality(0.899f), 1e-4f);
            // At 90% → 16% tip.
            Assert.AreEqual(0.16f, config.TipRateForQuality(0.90f), 1e-4f);
            // Below 98.5% → still 16%.
            Assert.AreEqual(0.16f, config.TipRateForQuality(0.984f), 1e-4f);
            // At 98.5% → 35% VIP tip.
            Assert.AreEqual(0.35f, config.TipRateForQuality(0.985f), 1e-4f);
        }

        [Test]
        public void AC_E1_ReputationDeltaAtQualityBoundaries()
        {
            // Below 84% → -0.15.
            Assert.AreEqual(-0.15f, config.ReputationDeltaForQuality(0.839f), 1e-4f);
            // At 84% → +0.08.
            Assert.AreEqual(0.08f, config.ReputationDeltaForQuality(0.84f), 1e-4f);
            // At 92% → +0.16.
            Assert.AreEqual(0.16f, config.ReputationDeltaForQuality(0.92f), 1e-4f);
            // At 98.5% → +0.25.
            Assert.AreEqual(0.25f, config.ReputationDeltaForQuality(0.985f), 1e-4f);
        }

        [Test]
        public void AC_E1_GradeLettersAtBoundaries()
        {
            Assert.AreEqual("D", config.GradeForQuality(0.839f));
            Assert.AreEqual("C", config.GradeForQuality(0.85f));
            Assert.AreEqual("B", config.GradeForQuality(0.90f));
            Assert.AreEqual("A", config.GradeForQuality(0.95f));
            Assert.AreEqual("S", config.GradeForQuality(0.985f));
        }

        [Test]
        public void DefaultValuesMatchGDD()
        {
            Assert.AreEqual(750, config.openingCredits);
            Assert.AreEqual(65, config.refillCost);
            Assert.AreEqual(0.58f, config.preRinseLimit);
            Assert.AreEqual(0.62f, config.foamCoverageMin);
            Assert.AreEqual(0.15f, config.finalRinseDirtMax);
            Assert.AreEqual(0.10f, config.finalRinseFoamMax);
            Assert.AreEqual(0.18f, config.foamQualityCoefficient);
            Assert.AreEqual(0.92f, config.automatedBayQualityCap);
            Assert.AreEqual(2.1f, config.heavyDispatchRep);
            Assert.AreEqual(3.8f, config.vipContractsRep);
        }
    }
}