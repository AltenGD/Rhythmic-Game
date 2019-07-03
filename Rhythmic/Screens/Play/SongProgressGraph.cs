using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using Object = Rhythmic.Beatmap.Properties.Level.Object.Object;

namespace Rhythmic.Screens.Play
{
    public class SongProgressGraph : SquareGraph
    {
        private IEnumerable<Object> objects;

        public IEnumerable<Object> Objects
        {
            set
            {
                objects = value;

                const int granularity = 200;
                Values = new int[granularity];

                if (!objects.Any())
                    return;

                var firstObj = objects.First().Time;
                var lastObj = objects.Max(o => (o?.Time + o?.TotalTime) ?? o.Time);

                if (lastObj == 0)
                    lastObj = objects.Last().Time;

                var interval = (lastObj - firstObj + 1) / granularity;

                foreach (var o in objects)
                {
                    var endTime = (o?.Time + o?.TotalTime) ?? o.Time;

                    Debug.Assert(endTime >= o.Time);

                    int startRange = (int)((o.Time - firstObj) / interval);
                    int endRange = (int)((endTime - firstObj) / interval);

                    for (int i = startRange; i <= endRange; i++)
                        Values[i]++;
                }
            }
        }
    }
}
