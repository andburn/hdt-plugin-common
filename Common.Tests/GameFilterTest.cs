using System;
using System.Collections.Generic;
using HDT.Plugins.Common.Models;
using HDT.Plugins.Common.Enums;
using HDT.Plugins.Common.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HDT.Plugins.Common.Tests
{
    [TestClass]
    public class GameFilterTest
    {
        private List<Game> games;
        private string now;

        [TestInitialize]
        public void SetUp()
        {
            now = "2015-01-10T19:23:44+00:00";
            games = new List<Game>()
            {
                new Game() {
                    Deck = new Deck(),
                    PlayerClass = PlayerClass.DRUID,
                    OpponentClass = PlayerClass.MAGE,
                    Region = Region.US,
                    Mode = GameMode.BRAWL,
                    Format = GameFormat.WILD,
                    StartTime = new DateTime(2015, 01, 25, 19, 03, 26),
                    EndTime = new DateTime(2015, 01, 25, 19, 09, 14)
                },
                new Game() {
                    Deck = new Deck(),
                    PlayerClass = PlayerClass.HUNTER,
                    OpponentClass = PlayerClass.MAGE,
                    Region = Region.EU,
                    Mode = GameMode.RANKED,
                    Format = GameFormat.STANDARD,
                    StartTime = DateTime.Now - new TimeSpan(2, 10, 0),
                    EndTime = DateTime.Now - new TimeSpan(2, 0, 0)
                }
            };
        }

        [TestMethod]
        public void DefaultFilter_ShouldHavePropsSetToAll()
        {
            var filter = new GameFilter();
            Assert.IsNull(filter.DeckId);
            Assert.AreEqual(Region.ALL, filter.Region);
            Assert.AreEqual(GameMode.ALL, filter.Mode);
            Assert.AreEqual(TimeFrame.ALL, filter.TimeFrame);
        }

        [TestMethod]
        public void DefaultFilter_ShouldDoNoFiltering()
        {
            var filter = new GameFilter();
            var filtered = filter.Apply(games);
            CollectionAssert.AreEquivalent(games, filtered);
        }

        [TestMethod]
        public void Apply_FilterByDeck()
        {
            var filter = new GameFilter(games[0].Deck.Id, Region.ALL, GameMode.ALL, TimeFrame.ALL, GameFormat.ANY);
            var filtered = filter.Apply(games);
            Assert.AreEqual(1, filtered.Count);
            Assert.AreEqual(games[0].Deck.Id, filtered[0].Deck.Id);
        }

        [TestMethod]
        public void Apply_FilterByRegion()
        {
            var filter = new GameFilter(null, Region.EU, GameMode.ALL, TimeFrame.ALL, GameFormat.ANY);
            var filtered = filter.Apply(games);
            Assert.AreEqual(1, filtered.Count);
            Assert.AreEqual(Region.EU, filtered[0].Region);
        }

        [TestMethod]
        public void Apply_FilterByMode()
        {
            var filter = new GameFilter(null, Region.ALL, GameMode.BRAWL, TimeFrame.ALL, GameFormat.ANY);
            var filtered = filter.Apply(games);
            Assert.AreEqual(1, filtered.Count);
            Assert.AreEqual(GameMode.BRAWL, filtered[0].Mode);
        }

        [TestMethod]
        public void Apply_FilterByFormat()
        {
            var filter = new GameFilter(null, Region.ALL, GameMode.ALL, TimeFrame.ALL, GameFormat.STANDARD);
            var filtered = filter.Apply(games);
            Assert.AreEqual(1, filtered.Count);
            Assert.AreEqual(GameFormat.STANDARD, filtered[0].Format);
        }

        [TestMethod]
        public void Apply_FilterByPlayerClass()
        {
            var filter = new GameFilter(null, Region.ALL, GameMode.ALL, TimeFrame.ALL, GameFormat.ANY, PlayerClass.DRUID, PlayerClass.ALL);
            var filtered = filter.Apply(games);
            Assert.AreEqual(1, filtered.Count);
            Assert.AreEqual(PlayerClass.DRUID, filtered[0].PlayerClass);
        }

        [TestMethod]
        public void Apply_FilterByOpponentClass()
        {
            var filter = new GameFilter(null, Region.ALL, GameMode.ALL, TimeFrame.ALL, GameFormat.ANY, PlayerClass.ALL, PlayerClass.MAGE);
            var filtered = filter.Apply(games);
            Assert.AreEqual(2, filtered.Count);
            Assert.AreEqual(PlayerClass.MAGE, filtered[0].OpponentClass);
            Assert.AreEqual(PlayerClass.MAGE, filtered[1].OpponentClass);
        }

        [TestMethod]
        public void Apply_FilterByTime()
        {
            var filter = new GameFilter(null, Region.ALL, GameMode.ALL, TimeFrame.TODAY, GameFormat.ANY);
            var filtered = filter.Apply(games);
            Assert.AreEqual(1, filtered.Count);
        }

        [TestMethod]
        public void ConvertTimeFrameToRange_ShouldUseNowIfParamIsAbsent()
        {
            var dt = DateTime.Now;
            var range = GameFilter.ConvertTimeFrameToRange(TimeFrame.TODAY);
            Assert.AreEqual(dt.Day, range.End.Day);
            Assert.AreEqual(dt.Month, range.End.Month);
            Assert.AreEqual(dt.Year, range.End.Year);
            Assert.AreEqual(dt.Hour, range.End.Hour);
            Assert.AreEqual(dt.Minute, range.End.Minute);
        }

        [TestMethod]
        public void ConvertTimeFrameToRange_All()
        {
            var range = GameFilter.ConvertTimeFrameToRange(TimeFrame.ALL, now);
            Assert.AreEqual(DateTime.MinValue, range.Start);
            Assert.AreEqual(DateTime.Parse(now), range.End);
        }

        [TestMethod]
        public void ConvertTimeFrameToRange_Today()
        {
            var start = "2015-01-10T00:00:00+00:00";
            var range = GameFilter.ConvertTimeFrameToRange(TimeFrame.TODAY, now);
            Assert.AreEqual(DateTime.Parse(start), range.Start);
            Assert.AreEqual(DateTime.Parse(now), range.End);
        }

        [TestMethod]
        public void ConvertTimeFrameToRange_Yesterday()
        {
            var start = "2015-01-09T00:00:00+00:00";
            var end = "2015-01-09T23:59:59+00:00";
            var range = GameFilter.ConvertTimeFrameToRange(TimeFrame.YESTERDAY, now);
            Assert.AreEqual(DateTime.Parse(start), range.Start);
            Assert.AreEqual(DateTime.Parse(end), range.End);
        }

        [TestMethod]
        public void ConvertTimeFrameToRange_Last24Hours()
        {
            var start = "2015-01-09T19:23:44+00:00";
            var range = GameFilter.ConvertTimeFrameToRange(TimeFrame.LAST_24_HOURS, now);
            Assert.AreEqual(DateTime.Parse(start), range.Start);
            Assert.AreEqual(DateTime.Parse(now), range.End);
        }

        [TestMethod]
        public void ConvertTimeFrameToRange_ThisWeek()
        {
            var start = "2015-01-05T00:00:00+00:00";
            var range = GameFilter.ConvertTimeFrameToRange(TimeFrame.THIS_WEEK, now);
            Assert.AreEqual(DateTime.Parse(start), range.Start);
            Assert.AreEqual(DateTime.Parse(now), range.End);
        }

        [TestMethod]
        public void ConvertTimeFrameToRange_PreviousWeek()
        {
            var start = "2014-12-29T00:00:00+00:00";
            var end = "2015-01-04T23:59:59+00:00";
            var range = GameFilter.ConvertTimeFrameToRange(TimeFrame.PREVIOUS_WEEK, now);
            Assert.AreEqual(DateTime.Parse(start), range.Start);
            Assert.AreEqual(DateTime.Parse(end), range.End);
        }

        [TestMethod]
        public void ConvertTimeFrameToRange_Last7Days()
        {
            var start = "2015-01-03T19:23:44+00:00";
            var range = GameFilter.ConvertTimeFrameToRange(TimeFrame.LAST_7_DAYS, now);
            Assert.AreEqual(DateTime.Parse(start), range.Start);
            Assert.AreEqual(DateTime.Parse(now), range.End);
        }

        [TestMethod]
        public void ConvertTimeFrameToRange_ThisMonth()
        {
            var start = "2015-01-01T00:00:00+00:00";
            var range = GameFilter.ConvertTimeFrameToRange(TimeFrame.THIS_MONTH, now);
            Assert.AreEqual(DateTime.Parse(start), range.Start);
            Assert.AreEqual(DateTime.Parse(now), range.End);
        }

        [TestMethod]
        public void ConvertTimeFrameToRange_PreviousMonth()
        {
            var start = "2014-12-01T00:00:00+00:00";
            var end = "2014-12-31T23:59:59+00:00";
            var range = GameFilter.ConvertTimeFrameToRange(TimeFrame.PREVIOUS_MONTH, now);
            Assert.AreEqual(DateTime.Parse(start), range.Start);
            Assert.AreEqual(DateTime.Parse(end), range.End);
        }

        [TestMethod]
        public void ConvertTimeFrameToRange_ThisYear()
        {
            var start = "2015-01-01T00:00:00+00:00";
            var range = GameFilter.ConvertTimeFrameToRange(TimeFrame.THIS_YEAR, now);
            Assert.AreEqual(DateTime.Parse(start), range.Start);
            Assert.AreEqual(DateTime.Parse(now), range.End);
        }

        [TestMethod]
        public void ConvertTimeFrameToRange_PreviousYear()
        {
            var start = "2014-01-01T00:00:00+00:00";
            var end = "2014-12-31T23:59:59+00:00";
            var range = GameFilter.ConvertTimeFrameToRange(TimeFrame.PREVIOUS_YEAR, now);
            Assert.AreEqual(DateTime.Parse(start), range.Start);
            Assert.AreEqual(DateTime.Parse(end), range.End);
        }

        [TestMethod]
        public void ConvertTimeFrameToRange_PreviousYear_LeapYear()
        {
            var now = "2017-05-24T11:24:16+00:00";
            var start = "2016-01-01T00:00:00+00:00";
            var end = "2016-12-31T23:59:59+00:00";
            var range = GameFilter.ConvertTimeFrameToRange(TimeFrame.PREVIOUS_YEAR, now);
            Assert.AreEqual(DateTime.Parse(start), range.Start);
            Assert.AreEqual(DateTime.Parse(end), range.End);
        }

        [TestMethod]
        public void DefaultTimeRange_IsMinMaxValues()
        {
            var range = new TimeRange();
            Assert.AreEqual(DateTime.MinValue, range.Start);
            Assert.AreEqual(DateTime.MaxValue, range.End);
        }

        [TestMethod]
        public void RankedRange_NoChange_WhenGameModeFilter_IsNotRanked()
        {
            var list = new List<Game>() {
                new Game() { Mode = GameMode.RANKED, Rank = 10 },
                new Game() { Mode = GameMode.RANKED, Rank = 5 }
            };
            var filter = new GameFilter() { Rank = new Tuple<int, int>(3, 1) };
            Assert.AreEqual(2, filter.Apply(list).Count);
        }

        [TestMethod]
        public void RankedRange_DefaultShouldIncludeAll()
        {
            var list = new List<Game>() {
                new Game() { Mode = GameMode.RANKED, Rank = 25 },
                new Game() { Mode = GameMode.RANKED, Rank = 0 }
            };
            var filter = new GameFilter() { Mode = GameMode.RANKED };
            Assert.AreEqual(2, filter.Apply(list).Count);
        }

        [TestMethod]
        public void RankedRange_LoShouldBeInclusive_HiShouldBeExclusive()
        {
            var list = new List<Game>() {
                new Game() { Mode = GameMode.RANKED, Rank = 10 },
                new Game() { Mode = GameMode.RANKED, Rank = 5 }
            };
            var filter = new GameFilter() { Mode = GameMode.RANKED, Rank = new Tuple<int, int>(10, 5) };
            var filtered = filter.Apply(list);
            Assert.AreEqual(1, filtered.Count);
            Assert.AreEqual(10, filtered[0].Rank);
        }
    }
}