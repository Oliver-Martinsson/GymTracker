namespace GymTracker.Core.Calculations;

public class OneRepMaxCalculator
{
    private const double EpleyRepDivisor = 30.0;

    public static double Epley(double weight, int reps)
    {
        if (weight <= 0 || reps <= 0)
        {
            return 0;
        }

        if (reps == 1)
        {
            return weight;
        }

        return weight * (1 + reps / EpleyRepDivisor);
    }
}