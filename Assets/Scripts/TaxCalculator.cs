﻿using UnityEngine;
using UnityEngine.UI;
using SpeechLib;

public class TaxCalculator : MonoBehaviour
{
    // Constant rate for the Medicare Levy
    const double MEDICARE_LEVY = 0.02;
    public Dropdown TimePeriodDropdown;
    public InputField GSIField;
    public Toggle ToggleTTS;
    public Text OutputNetIncome;
    public Text OutputMedicareLevy;
    public Text OutputTaxPaid;
    public Text Error;
    // Variables
    bool textToSpeechEnabled = true;

    private void Start()
    {
        Speak("Welcome to the A.T.O. Tax Calculator");
    }

    // Run this function on the click event of your 'Calculate' button
    public void Calculate()
    {
        // Initialisation of variables
        double medicareLevyPaid = 0;
        double incomeTaxPaid = 0;

        // Input
        double grossSalaryInput = GetGrossSalary();
        int salaryPayPeriod = GetSalaryPayPeriod();

        // Calculations
        double grossYearlySalary = CalculateGrossYearlySalary(grossSalaryInput, salaryPayPeriod);
        double netIncome = CalculateNetIncome(grossYearlySalary, ref medicareLevyPaid, ref incomeTaxPaid);

        // Output
        OutputResults(medicareLevyPaid, incomeTaxPaid, netIncome);
    }

    private double GetGrossSalary()
    {
        // Get from user. E.g. input box
        // Validate the input (ensure it is a positive, valid number)
        if (double.TryParse(GSIField.text, out double grossSalaryInput))
        {
            return grossSalaryInput;
        }
        else
        {
            return 0;
        }
    }

    private int GetSalaryPayPeriod()
    {
        int timeperiod = TimePeriodDropdown.value;
        if (timeperiod == 0) { return 1; }
        if (timeperiod == 1) { return 12; }
        if (timeperiod == 2) { return 26; }
        if (timeperiod == 3) { return 52; }

        return 0;
    }

    private double CalculateGrossYearlySalary(double grossSalaryInput, int salaryPayPeriod)
    {
        return grossSalaryInput * salaryPayPeriod;
    }

    private double CalculateNetIncome(double grossYearlySalary, ref double medicareLevyPaid, ref double incomeTaxPaid)
    {
        medicareLevyPaid = CalculateMedicareLevy(grossYearlySalary);
        incomeTaxPaid = CalculateIncomeTax(grossYearlySalary);
        double netIncome = grossYearlySalary - (incomeTaxPaid + medicareLevyPaid);
        return netIncome;
    }

    private double CalculateMedicareLevy(double grossYearlySalary)
    {
        double medicareLevyPaid = (grossYearlySalary * MEDICARE_LEVY);
        return medicareLevyPaid;
    }

    private double CalculateIncomeTax(double grossYearlySalary)
    {
        if (grossYearlySalary >= 1 && grossYearlySalary <= 18200)
        {
            return grossYearlySalary;
        }
        else if (grossYearlySalary >= 18201 && grossYearlySalary <= 37000)
        {
            return (grossYearlySalary - (grossYearlySalary * 0.19));
        }
        else if (grossYearlySalary >= 37001 && grossYearlySalary <= 90000)
        {
            return (grossYearlySalary - (grossYearlySalary * 0.325));
        }
        else if (grossYearlySalary >= 90001 && grossYearlySalary <= 180000)
        {
            return (grossYearlySalary - (grossYearlySalary * 0.37));
        }
        else if (grossYearlySalary >= 180001)
        {
            return (grossYearlySalary - (grossYearlySalary * 0.45));
        }
        else
        {
            return 0;
        }
    }

    private void OutputResults(double medicareLevyPaid, double incomeTaxPaid, double netIncome)
    {
        // Output the following to the GUI
        OutputMedicareLevy = "Medicare levy paid: $" + medicareLevyPaid.ToString("F2");
        OutputTaxPaid = "Income tax paid: $" + incomeTaxPaid.ToString("F2");
        OutputNetIncome = "Net income: $" + netIncome.ToString("F2");
    }

    // Text to Speech
    private void Speak(string textToSpeak)
    {
        if(textToSpeechEnabled)
        {
            SpVoice voice = new SpVoice();
            voice.Speak(textToSpeak);
        }
    }
}
