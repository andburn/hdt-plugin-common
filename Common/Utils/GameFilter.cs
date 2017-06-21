﻿using System;
using System.Collections.Generic;
using System.Linq;
using HDT.Plugins.Common.Models;
using HDT.Plugins.Common.Enums;

namespace HDT.Plugins.Common.Utils
{
    public class GameFilter
    {
        public Guid? DeckId { get; set; }
        public PlayerClass PlayerClass { get; set; }
        public PlayerClass OpponentClass { get; set; }
        public Region Region { get; set; }
        public GameMode Mode { get; set; }
        public GameFormat Format { get; set; }
        public TimeFrame TimeFrame { get; set; }

        public GameFilter()
        {
            DeckId = null;
            Region = Region.ALL;
            Mode = GameMode.ALL;
            TimeFrame = TimeFrame.ALL;
            Format = GameFormat.ANY;
            PlayerClass = PlayerClass.ALL;
            OpponentClass = PlayerClass.ALL;
        }

        public GameFilter(Guid? deck, Region region, GameMode mode, TimeFrame time,
            GameFormat format)
        {
            DeckId = deck;
            Region = region;
            Mode = mode;
            TimeFrame = time;
            Format = format;
            PlayerClass = PlayerClass.ALL;
            OpponentClass = PlayerClass.ALL;
        }

        public GameFilter(Guid? deck, Region region, GameMode mode, TimeFrame time, 
            GameFormat format, PlayerClass pClass, PlayerClass oClass)
        {
            DeckId = deck;
            Region = region;
            Mode = mode;
            TimeFrame = time;
            Format = format;
            PlayerClass = pClass;
            OpponentClass = oClass;
        }

        public List<Game> Apply(List<Game> games)
        {
            IEnumerable<Game> filtered = games;
            // filter by deck first if needed
            if (DeckId != null)
            {
                if (DeckId == Deck.None.Id)
                {
                    filtered = filtered.Where(g => g.Deck == null);
                }
                else
                {
                    filtered = filtered.Where(g => g.Deck != null && DeckId.Equals(g.Deck.Id));
                }
            }
            // player class filter (only useful if deck is null)
            if (!PlayerClass.Equals(PlayerClass.ALL))
            {
                filtered = filtered.Where(g => g.PlayerClass == PlayerClass);
            }
            // opponent class filter
            if (!OpponentClass.Equals(PlayerClass.ALL))
            {
                filtered = filtered.Where(g => g.OpponentClass == OpponentClass);
            }
            // format filter
            if (!Format.Equals(GameFormat.ANY))
            {
                filtered = filtered.Where(g => g.Format == Format);
            }
            // region filter
            if (!Region.Equals(Region.ALL))
            {
                filtered = filtered.Where(g => g.Region == Region);
            }
            // game mode filter
            if (!Mode.Equals(GameMode.ALL))
            {
                filtered = filtered.Where(g => g.Mode == Mode);
            }
            // time filter
            var range = ConvertTimeFrameToRange(TimeFrame);
            filtered = filtered.Where(g => g.StartTime >= range.Start && g.EndTime <= range.End);

            // finally sort by time
            // TODO should this be filters job?
            return filtered.OrderByDescending(g => g.EndTime).ToList();
        }

        /// <summary>
        /// Convert a TimeFrame into TimeRange with a Start and End
        /// </summary>
        /// <param name="time">A TimeFrame</param>
        /// <param name="now">An optional string representing the current date/time (default: DateTime.Now)</param>
        /// <returns></returns>
        public static TimeRange ConvertTimeFrameToRange(TimeFrame time, string now = null)
        {
            DateTime current;
            try
            {
                current = DateTime.Parse(now);
            }
            catch (Exception)
            {
                // Exception means null or failed to parse, ok to use default
                current = DateTime.Now;
            }

            var startTime = new DateTime(current.Year, current.Month, current.Day, 0, 0, 0);
            var endTime = current;

            switch (time)
            {
                case TimeFrame.YESTERDAY:
                    startTime -= new TimeSpan(1, 0, 0, 0);
                    endTime = new DateTime(current.Year, current.Month, current.Day, 23, 59, 59)
                        - new TimeSpan(1, 0, 0, 0);
                    break;

                case TimeFrame.LAST_24_HOURS:
                    startTime = current - new TimeSpan(1, 0, 0, 0);
                    endTime = current;
                    break;

                case TimeFrame.THIS_WEEK:
                    startTime -= new TimeSpan(((int)(current.DayOfWeek) - 1), 0, 0, 0);
                    endTime = current;
                    break;

                case TimeFrame.PREVIOUS_WEEK:
                    startTime -= new TimeSpan(7 + ((int)(current.DayOfWeek) - 1), 0, 0, 0);
                    endTime = startTime + new TimeSpan(6, 23, 59, 59);
                    break;

                case TimeFrame.LAST_7_DAYS:
                    startTime = current - new TimeSpan(7, 0, 0, 0);
                    endTime = current;
                    break;

                case TimeFrame.THIS_MONTH:
                    startTime -= new TimeSpan(current.Day - 1, 0, 0, 0);
                    endTime = current;
                    break;

                case TimeFrame.PREVIOUS_MONTH:
                    var prevMonth = current.AddMonths(-1);
                    var daysInMonth = DateTime.DaysInMonth(prevMonth.Year, prevMonth.Month);
                    startTime -= new TimeSpan(current.Day - 1 + daysInMonth, 0, 0, 0);
                    endTime = startTime + new TimeSpan(daysInMonth - 1, 23, 59, 59);
                    break;

                case TimeFrame.THIS_YEAR:
                    startTime -= new TimeSpan(current.DayOfYear - 1, 0, 0, 0);
                    endTime = current;
                    break;

                case TimeFrame.PREVIOUS_YEAR:
                    startTime = new DateTime(current.Year - 1, 1, 1, 0, 0, 0);
                    endTime = new DateTime(current.Year - 1, 12, 31, 23, 59, 59);
                    break;

                case TimeFrame.ALL:
                    endTime = current;
                    startTime = DateTime.MinValue;
                    break;

                case TimeFrame.TODAY:
                default:
                    // use defaults
                    break;
            }

            return new TimeRange(startTime, endTime);
        }
    }

    public class TimeRange
    {
        public DateTime Start { get; private set; }
        public DateTime End { get; private set; }

        public TimeRange()
        {
            Start = DateTime.MinValue;
            End = DateTime.MaxValue;
        }

        public TimeRange(DateTime start, DateTime end)
        {
            Start = start;
            End = end;
        }
    }
}