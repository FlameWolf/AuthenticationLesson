using System;

namespace AuthenticationLesson
{
	public class WeatherForecast
	{
		public DateTime Date { set; get; }

		public int TemperatureC { set; get; }

		public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

		public string Summary { set; get; }
	}
}