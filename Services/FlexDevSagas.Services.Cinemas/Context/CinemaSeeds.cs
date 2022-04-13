using FlexDevSagas.Services.Cinemas.Entities;

namespace FlexDevSagas.Services.Cinemas.Context
{
    public class CinemaSeeds
    {
        public static void Seed(CinemaContext context)
        {
            if (context.Cinemas.Count() > 0)
            {
                return;
            }

            var cinema = new Cinema()
            {
                Name = "Cinema 1"
            };
            context.Cinemas.Add(cinema);

            var auditorium1 = new Auditorium()
            {
                Capacity = 50,
                Number = 1,
                Cinema = cinema,
            };
            context.Auditoriums.Add(auditorium1);

            for(int index = 1; index <= 10; index++)
            {
                var row = new Row()
                {
                    Auditoirum = auditorium1,
                    Number = index
                };

                for(int seatIndex = 1; seatIndex <= 5; seatIndex++)
                {
                    var seat = new Seat()
                    {
                        Number = seatIndex,
                        Row = row
                    };
                    context.Seats.Add(seat);
                }
                context.Rows.Add(row);
            }

            var auditorium2 = new Auditorium()
            {
                Capacity = 50,
                Number = 1,
                Cinema = cinema,
            };
            context.Auditoriums.Add(auditorium2);

            for (int index = 1; index <= 5; index++)
            {
                var row = new Row()
                {
                    Auditoirum = auditorium2,
                    Number = index
                };

                for (int seatIndex = 1; seatIndex <= 10; seatIndex++)
                {
                    var seat = new Seat()
                    {
                        Number = seatIndex,
                        Row = row
                    };
                    context.Seats.Add(seat);
                }
                context.Rows.Add(row);
            }

            context.SaveChanges();
        }
    }
}
