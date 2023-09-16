using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace dji_marks_sync {
    public static class Utils {
        public static DateTime GpsTimeToUtc(int weekNumber, double seconds) {
            DateTime datum = new(1980, 1, 6, 0, 0, 0);
            DateTime week = datum.AddDays(weekNumber * 7);
            DateTime time = week.AddSeconds(seconds);
            return time;
        }
    }

    public struct Offset {
        public double N { get; set; }
        public double E { get; set; }
        public double V { get; set; }
    }

    public struct TimestampMark {
        public int ImageSequence { get; set; }
        public double GpsSeconds { get; set; }
        public int GpsWeek { get; set; }
        public Offset AntennaPcOffset { get; set; }
        public DateTimeOffset DateUtc { get; set; }
    }

    public struct ImageMark {
        public string ImageFileName {get;set;}
        public string? ImageFileExtension {get;set;}
        public DateTime Date {get;set;}
    }

    public class Parser {
        private static System.Globalization.CultureInfo _culture = System.Globalization.CultureInfo.GetCultureInfo("en-US");
        private static System.Globalization.NumberStyles _numberStyles = System.Globalization.NumberStyles.Any;

        public List<string>? _ReadMrkFile(ref string path) {
            List<string>? result = null;
            using StreamReader reader = File.OpenText(path);
            string? line = null;

            try {
                line = reader.ReadLine();
                result = new(1000);
            } catch {
                return result;
            }

            while (line != null) {
                result.Add(line);
                line = reader.ReadLine();
            }

            return result;
        }

        public List<TimestampMark>? ParseMrkFile(string path, TimeSpan? timeOffset = null) {
            var data = _ReadMrkFile(ref path);
            if (data == null) {
                return null;
            }

            var result = new List<TimestampMark>(data.Count);
            TimeSpan _timeOffset = timeOffset ?? new TimeSpan();
            Regex regex = new(@"(?<imgSequence>[0-9]+)[\t\s]+(?<gpsSeconds>[0-9]+\.[0-9]+)[\t\s]+\[(?<gpsWeek>[0-9]+)\]");
            data.ForEach((line) => {
                var match = regex.Match(line);
                if (match.Success) {
                    TimestampMark newItem = new() {
                        ImageSequence = int.Parse(match.Groups["imgSequence"].ValueSpan, System.Globalization.NumberStyles.Integer),
                        GpsSeconds = double.Parse(match.Groups["gpsSeconds"].ValueSpan, System.Globalization.NumberStyles.Float, _culture),
                        GpsWeek = int.Parse(match.Groups["gpsWeek"].ValueSpan, System.Globalization.NumberStyles.Integer),
                    };
                    var time = Utils.GpsTimeToUtc(newItem.GpsWeek, newItem.GpsSeconds);
                    if (timeOffset != null) {
                        time = time.Add(timeOffset.Value);
                    }
                    newItem.DateUtc = new DateTimeOffset(Utils.GpsTimeToUtc(newItem.GpsWeek, newItem.GpsSeconds), _timeOffset);
                    result.Add(newItem);
                }
            });

            return result;
        }

        public void ParseImageMark() {
            
        }

        public void TeoboxMrkFileParser(string path) {
            var data = _ReadMrkFile(ref path);
            if (data == null) {
                return;
            }

            var result = new List<TimestampMark>(data.Count);
            const string imgNamePattern = @"(?<imgFileName>DJI_(?<imgDate>[0-9]+)_(?<imgNumber>[0-9]+)(?<imgExtension>\.(?:JPG)|(?:PNG))?)";
            const string imgDatePattern = @"(?<date>[0-9]{4}/[0-9]{1,2}/[0-9]{1,2})\s+(?<time>[0-9]{1,2}\:[0-9]{1,2}\.[0-9]+)";
            Regex regex = new(imgNamePattern + @"\s+" + imgDatePattern);
            data.ForEach((line) => {
                var match = regex.Match(line);
                if (match.Success) {
                }
            });

            return;
        }

    }

    public class Editor {
        public List<TimestampMark>? EditMrkFile(string path, TimeSpan? timeOffset = null) {
            Parser parser = new();
            var data = parser._ReadMrkFile(ref path);
            if (data == null) {
                return null;
            }

            var result = new List<TimestampMark>(data.Count);
            TimeSpan _timeOffset = timeOffset ?? new TimeSpan();
            Regex regex = new(@"(?<imgSequence>[0-9]+)[\t\s]+(?<gpsSeconds>[0-9]+\.[0-9]+)[\t\s]+\[(?<gpsWeek>[0-9]+)\]");
            data.ForEach((line) => {
                var match = regex.Match(line);
                if (match.Success) {
                    TimestampMark newItem = new() {
                        ImageSequence = int.Parse(match.Groups["imgSequence"].ValueSpan, System.Globalization.NumberStyles.Integer),
                        GpsSeconds = double.Parse(match.Groups["gpsSeconds"].ValueSpan, System.Globalization.NumberStyles.Float, _culture),
                        GpsWeek = int.Parse(match.Groups["gpsWeek"].ValueSpan, System.Globalization.NumberStyles.Integer),
                    };
                    var time = Utils.GpsTimeToUtc(newItem.GpsWeek, newItem.GpsSeconds);
                    if (timeOffset != null) {
                        time = time.Add(timeOffset.Value);
                    }
                    newItem.DateUtc = new DateTimeOffset(Utils.GpsTimeToUtc(newItem.GpsWeek, newItem.GpsSeconds), _timeOffset);
                    result.Add(newItem);
                }
            });

            return result;
        }
    }
}