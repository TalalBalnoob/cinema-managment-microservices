// ...existing code...
namespace TheaterService.Statics;

public enum HallNumbers {
	A = 1, B = 2, C = 3, D = 4, E = 5, F = 6, G = 7, H = 8, I = 9, J = 10,
	K = 11, L = 12, M = 13, N = 14, O = 15, P = 16, Q = 17, R = 18, S = 19, T = 20,
	U = 21, V = 22, W = 23, X = 24, Y = 25, Z = 26
}

public static class HallNumbersHelper {
	public static HallNumbers FromInt(int n) {
		if (n < 1 || n > 26) throw new ArgumentOutOfRangeException(nameof(n), "Value must be between 1 and 26.");
		return (HallNumbers)n;
	}

	public static string IntToLetter(int n) => FromInt(n).ToString();

	public static string ToLetter(this HallNumbers h) => h.ToString();
}
