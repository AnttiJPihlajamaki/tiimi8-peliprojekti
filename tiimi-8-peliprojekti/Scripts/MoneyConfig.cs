using System;
using Godot;

public static class MoneyConfig // String references for all different inputs
{
	public static string MoneyConversion(float money)
	{
		string symbol = "";

		if(money >= Mathf.Pow(10, 20))
		{
			int power = 0;
			for(int i = 0; money > Mathf.Pow(10,3); money /= 10)
			{
				i++;
				power = i;
			}
			symbol = "E+"+power;
		}
		if(money >= Mathf.Pow(10, 17))
		{
			money /= Mathf.Pow(10, 15);
			symbol = "Q";
		}
		if(money >= Mathf.Pow(10, 14))
		{
			money /= Mathf.Pow(10, 12);
			symbol = "T";
		}
		else if(money >= Mathf.Pow(10, 11))
		{
			money /= Mathf.Pow(10, 9);
			symbol = "B";
		}
		else if(money >= Mathf.Pow(10, 8))
		{
			money /= Mathf.Pow(10, 6);
			symbol = "M";
		}
		else if(money >= Mathf.Pow(10, 5))
		{
			money /= Mathf.Pow(10, 3);
			symbol = "K";
		}

		return ""+Mathf.Floor(money)+" "+symbol;
	}
}
