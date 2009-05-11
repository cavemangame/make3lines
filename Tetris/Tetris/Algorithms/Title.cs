namespace XnaTetris.Algorithms
{
  public class Title
  {
    public static string GetTitle(long score)
    {
      if (score < 2500)
        return "Неизвестный";
      if (score < 5000)
        return "Ученик";
      if (score < 10000)
        return "Подмастерье";
      if (score < 20000)
        return "Лучший";
      if (score < 30000)
        return "Маг";
      if (score < 50000)
        return "Профессор";
      if (score < 70000)
        return "Мастер";
      if (score < 100000)
        return "Повелитель стихий";
      if (score < 150000)
        return "Старейший";
      return "Полубог";
    }
  }
}
