using System;

namespace FraminghamnBN
{
  class FraminghamRiskScore
  {
    public static int scorePoints(int sex, int age, int totchol, int smoke, int hdl, double sysbp, int bpmeds)
    {
      int points = 0;
      //Men
      if (sex == 1)
      {
        if (age <= 39)
        {
          if (age <= 34) points -= 9; else points -= 4;

          if (smoke == 1) points += 8;

          if (totchol >= 280) points += 11;
          else if (totchol >= 240) points += 9;
          else if (totchol >= 200) points += 7;
          else if (totchol >= 160) points += 4;
        }
        else if (age <= 49)
        {
          if (age >= 44) points += 3;
          if (smoke == 1) points += 5;

          if (totchol >= 280) points += 8;
          else if (totchol >= 240) points += 6;
          else if (totchol >= 200) points += 5;
          else if (totchol >= 160) points += 3;
        }
        else if (age <= 59)
        {
          if (age <= 54) points += 6; else points += 8;
          if (smoke == 1) points += 3;

          if (totchol >= 280) points += 5;
          else if (totchol >= 240) points += 4;
          else if (totchol >= 200) points += 3;
          else if (totchol >= 160) points += 2;
        }
        else if (age <= 69)
        {
          if (age <= 64) points += 10; else points += 11;
          if (smoke == 1) points += 1;

          if (totchol >= 280) points += 3;
          else if (totchol >= 240) points += 2;
          else if (totchol >= 160) points += 1;
        }
        else if (age <= 81)
        {
          if (age <= 74) points += 12; else points += 13;
          if (smoke == 1) points += 1;
          if (totchol >= 240) points += 1;
        }

        if (hdl < 40) points += 2;
        else if (hdl <= 49) points += 1;
        else if (hdl > 60) points -= 1;

        if (bpmeds == 0)
        {
          if (sysbp >= 130 && sysbp <= 159) points += 1;
          else if (sysbp >= 160) points += 2;
        }
        else
        {
          if (sysbp >= 160) points += 3;
          else if (sysbp >= 130) points += 2;
          else if (sysbp >= 120) points += 1;
        }

      }
      //Women
      else
      {
        if (age <= 39)
        {
          if (age <= 34) points -= 7; else points -= 3;

          if (smoke == 1) points += 9;

          if (totchol >= 280) points += 13;
          else if (totchol >= 240) points += 11;
          else if (totchol >= 200) points += 8;
          else if (totchol >= 160) points += 4;
        }
        else if (age <= 49)
        {
          if (age >= 44) points += 3;
          if (smoke == 1) points += 7;

          if (totchol >= 280) points += 10;
          else if (totchol >= 240) points += 8;
          else if (totchol >= 200) points += 6;
          else if (totchol >= 160) points += 3;
        }
        else if (age <= 59)
        {
          if (age <= 54) points += 6; else points += 8;
          if (smoke == 1) points += 4;

          if (totchol >= 280) points += 7;
          else if (totchol >= 240) points += 5;
          else if (totchol >= 200) points += 4;
          else if (totchol >= 160) points += 2;
        }
        else if (age <= 69)
        {
          if (age <= 64) points += 10; else points += 12;
          if (smoke == 1) points += 2;

          if (totchol >= 280) points += 4;
          else if (totchol >= 240) points += 3;
          else if (totchol >= 200) points += 2;
          else if (totchol >= 160) points += 1;
        }
        else if (age <= 81)
        {
          if (age <= 74) points += 14; else points += 16;
          if (smoke == 1) points += 1;

          if (totchol >= 280) points += 2;
          else if (totchol >= 160) points += 1;
        }

        if (hdl < 40) points += 2;
        else if (hdl <= 49) points += 1;
        else if (hdl > 60) points -= 1;

        if (bpmeds == 0)
        {
          if (sysbp >= 160) points += 4;
          else if (sysbp >= 140) points += 3;
          else if (sysbp >= 130) points += 2;
          else if (sysbp >= 120) points += 1;
        }
        else
        {
          if (sysbp >= 160) points += 6;
          else if (sysbp >= 130) points += 5;
          else if (sysbp >= 130) points += 4;
          else if (sysbp >= 120) points += 3;
        }
      }
      return points;
    }

    public static int risk(string curstr, char delimetr)
    {
      int riskscore = 0;

      string[] curArr = curstr.Split(delimetr);
      int sex = Int32.Parse(curArr[1]);
      int totchol = 0;
      if (curArr[2] != "") totchol = Int32.Parse(curArr[2]);
      int age = Int32.Parse(curArr[3]);
      double sysbp = Convert.ToDouble(curArr[4].Replace(".", ","));
      int smoke = Int32.Parse(curArr[6]);
      int hdl = 0;
      if (curArr[21] != "") hdl = Int32.Parse(curArr[21]);
      int bpmeds = 0;
      if (curArr[10] != "") bpmeds = Int32.Parse(curArr[10]);

      riskscore = scorePoints(sex, age, totchol, smoke, hdl, sysbp, bpmeds);
      if (sex == 1)
      {
        if (riskscore == 0) return 0;
        else if (riskscore <= 4) return 1;
        else if (riskscore <= 6) return 2;
        else if (riskscore == 7) return 3;
        else if (riskscore == 8) return 4;
        else if (riskscore == 9) return 5;
        else if (riskscore == 10) return 6;
        else if (riskscore == 11) return 8;
        else if (riskscore == 12) return 10;
        else if (riskscore == 13) return 12;
        else if (riskscore == 14) return 16;
        else if (riskscore == 15) return 20;
        else if (riskscore == 16) return 25;
        else return 30;
      }
      else
      {
        if (riskscore <= 9) return 0;
        else if (riskscore <= 12) return 1;
        else if (riskscore <= 14) return 2;
        else if (riskscore == 15) return 3;
        else if (riskscore == 16) return 4;
        else if (riskscore == 17) return 5;
        else if (riskscore == 18) return 6;
        else if (riskscore == 19) return 8;
        else if (riskscore == 20) return 11;
        else if (riskscore == 21) return 14;
        else if (riskscore == 22) return 17;
        else if (riskscore == 23) return 22;
        else if (riskscore == 24) return 27;
        else return 30;
      }
    }
  }
}