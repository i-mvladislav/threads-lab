using Example08;

PrintInstraction();

var booking = new Booking();
booking.Processing();


void PrintInstraction()
{
    Console.WriteLine(@$"""1"" - забронировать место");
    Console.WriteLine(@$"""2"" - отменить бронь");
    Console.WriteLine(@$"""q"" - завершить работу");
}