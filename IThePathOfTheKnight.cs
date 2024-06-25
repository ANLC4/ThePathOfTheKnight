using System.Threading.Tasks;
using ThePathofKnight;

public interface IThePathOfTheKnight
{
    Task<TheKnightPathRecord> FindShortest((int, int) start, (int, int) end);
}