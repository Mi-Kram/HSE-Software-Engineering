namespace Main.Models.Timing
{
    /// <summary>
    /// Запись: содержит информацию о времени исполнения обработчика под меткой Label.
    /// </summary>
    /// <param name="Label"></param>
    /// <param name="Times"></param>
    public record TimingReportItem(string Label, List<TimeSpan> Times)
    { }
}
