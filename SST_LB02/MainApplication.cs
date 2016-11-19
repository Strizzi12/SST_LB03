using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Pres;

namespace SST_LB03
{
    class MainApplication
    {
		
		static void Main(string[] args)
        {
			//#region FUNCTION TEST CALL
			////clear existing data of directory
			//DataMaintenance.Intf_clearData();
   //         //--------------------------------------------------------------------------
   //         int VEcusID, FScusID = 0;
   //         // Customer functions
   //         VEcusID = CustomerWrapper.Intf_createCustomer("Mike", "Thomas", "5020", "Salzburg", "Breitenfelderstrasse", 47, "13.11.1992");

   //         CustomerWrapper.Intf_updateCustomer(VEcusID, "Mike", "Anders", "5020", "AnotherPlace", "AnotherStreet", 1111, "11.11.1111");

   //         CustomerWrapper.Intf_deleteCustomer(VEcusID);

   //         VEcusID = CustomerWrapper.Intf_createCustomer("Betty", "Katzian", "5020", "Salzburg", "Breitenfelderstrasse", 47, "13.11.1992");
			////--------------------------------------------------------------------------
			//// Account functions
			//int VEaccID, FSaccID = 0;
   //         VEaccID = AccountWrapper.Intf_createAccount(0, 1000, "TestNameAcc");

   //         //Account edit
   //         try
   //         {
   //             //interface throws notimplex bc 2nd party dll does not include this functionality
   //             AccountWrapper.Intf_editAccount(FSaccID, 1);
   //         }
   //         catch (Exception ex)
   //         {
   //             Debug.Print(ex.Message);
   //         }

   //         //Account deletion
   //         try
   //         {
   //             //interface throws notimplex bc 2nd party dll does not include this functionality
   //             AccountWrapper.Intf_deleteAccount(FSaccID);
   //         }
   //         catch (Exception ex)
   //         {
   //             Debug.Print(ex.Message);
   //         }

   //         ////create transactions for the accounts
   //         int VEaccID2, FSaccID2, VEaccID3, FSaccID3 = 0;
   //         VEaccID2 = AccountWrapper.Intf_createAccount(0, 1000, "TestNameAcc");
   //         FSaccID2 = AccountWrapper.Intf_createAccount(0, 1000, "TestNameAcc");
			//VEaccID3 = AccountWrapper.Intf_createAccount(0, 1000, "TestNameAcc");
			//FSaccID3 = AccountWrapper.Intf_createAccount(0, 1000, "TestNameAcc");

			////Attach/Dettach customer to account
			//AccountWrapper.Intf_attachAccount(FSaccID2, FScusID);

   //         AccountWrapper.Intf_dettachAccount(FSaccID2, FScusID);

   //         AccountWrapper.Intf_attachAccount(FSaccID2, FScusID);

   //         //Creating transactions
   //         TransactionWrapper.Intf_transfer(FScusID, FSaccID2, FSaccID3, 100, 0);

			//TransactionWrapper.Intf_deposit(FSaccID2, 500);

			//TransactionWrapper.Intf_withdraw(FSaccID2, 500);

   //         //create a bankstatement
   //         AccountWrapper.Intf_createBankStatement(FSaccID2);

			//TransactionWrapper.Intf_transfer(FScusID, FSaccID2, FSaccID3, 100, 0);

   //         //create a bankstatement
   //         AccountWrapper.Intf_createBankStatement(FSaccID2);
			//#endregion

			DataMaintenance.Intf_clearData();
			Console.WriteLine("\nThread schläft für 5 Sekunden, bitte warten!");
			Thread.Sleep(5000);
			Console.Clear();
            AccountWrapper.Intf_createAccount(0, 1000.00, "FirstAccount");
            AccountWrapper.Intf_createAccount(0, 1000.00, "SecondAccount");
            AccountWrapper.Intf_createAccount(0, 1000.00, "ThirdAccount");
            bool running = true;
			Console.WriteLine("Willkommen zum einfachen BankClient.\nIm Folgenden werden die eingebauten Kommandos  und mit welcher Zahl sie aufgerufen werden können erklärt.");
			hilfe();
            TransactionFunctions.Receive(); //Needs to be called atleast once, so that the consumer can listen to incoming Transactions
            while (running)
			{
				Console.WriteLine("Bitte Zahl eingeben: ");
				running = eingabeAusfuehren(Console.ReadLine());
			}
        }

		static void hilfe()
		{
			Console.WriteLine("Bitte geben sie die jeweilige Zahl ein um die entsprechende Funktion aufzurufen!");
			Console.WriteLine("Neuen Kunden erstellen: 1");
			Console.WriteLine("Kunden bearbeiten: 2");
			Console.WriteLine("Kunden löschen: 3");
			Console.WriteLine("Neues Konto erstellen: 4");
			Console.WriteLine("Konto bearbeiten: 5");
			Console.WriteLine("Konto löschen: 6");
			Console.WriteLine("Kontoauszug erstellen: 7");
			Console.WriteLine("Neue Überweisung: 8");
			Console.WriteLine("Neue Remote - Überweisung: 9");
			Console.WriteLine("Einzahlen: 10");
			Console.WriteLine("Abheben: 11");
			Console.WriteLine("Wartungsaufgaben (bedeutet, löschen aller Daten, für Simulationszwecke): 12");
			Console.WriteLine("Hilfe anzeigen: 0");
			Console.WriteLine("Exit: exit\n");
		}

		static bool eingabeAusfuehren(string s)
		{
			string a = "";
			int id = 0;
			switch (s)
			{
				case "0":	//Hilfe ausgeben
					hilfe();
					return true;

				case "1":	//Neuen Kunden erstellen
					try
					{
						Console.WriteLine("Um einen neuen Kunden anzulegen, bitte folgende Daten mit Beistrich und Leerzeichen (, ) getrennt eingeben.");
						Console.WriteLine("Vorname, Nachname, Ort, PLZ, Strasse, Hausnummer, Geburtstag\nEingabe: ");
						a = Console.ReadLine();
						//Bei string.split wird auch immer das Trennzeichen in das array kopiert.
						id = CustomerWrapper.Intf_createCustomer(a.Split(',', ' ')[0],	//Vorname
							a.Split(',', ' ')[2],										//Nachname
							a.Split(',', ' ')[4],										//Ort
							a.Split(',', ' ')[6],                                       //PLZ
							a.Split(',', ' ')[8],                                       //Strasse
							Int32.Parse(a.Split(',', ' ')[10]),                         //Hausnummer
							a.Split(',', ' ')[12]);                                     //Geburtsdatum

						if (id >= 0)
							Console.WriteLine("Deine Benutzer ID = " + id + ", diese bitte merken.");
						else
							Console.WriteLine("Falsche Eingaben, bitte wiederholen.");
					}
					catch (Exception e)
					{
						Console.WriteLine("Error: " + e.Message);
					}
					return true;

				case "2":	//Kunden bearbeiten
					try
					{
						Console.WriteLine("Um einen Kunden zu bearbeiten, bitte folgende Daten mit Beistrich und Leerzeichen (, ) getrennt eingeben.");
						Console.WriteLine("Kundennummer, Vorname, Nachname, Ort, PLZ, Strasse, Hausnummer, Geburtstag\nEingabe: ");
						a = Console.ReadLine();
						//Bei string.split wird auch immer das Trennzeichen in das array kopiert.
						id = CustomerWrapper.Intf_updateCustomer(Int32.Parse(a.Split(',', ' ')[0]), //Kundennummer
							a.Split(',', ' ')[2],													//Vorname
							a.Split(',', ' ')[4],													//Nachname
							a.Split(',', ' ')[6],													//Ort
							a.Split(',', ' ')[8],													//PLZ
							a.Split(',', ' ')[10],													//Strasse
							Int32.Parse(a.Split(',', ' ')[12]),										//Hausnummer
							a.Split(',', ' ')[14]);													//Geburtsdatum

						if (id >= 0)
							Console.WriteLine("Erfolgreich geändert.");
						else
							Console.WriteLine("Falsche Eingaben, bitte wiederholen.");
					}
					catch (Exception e)
					{
						Console.WriteLine("Error: " + e.Message);
					}
					return true;

				case "3":	//Kunden löschen
					try
					{
						Console.WriteLine("Um einen Kunden zu löschen, bitte die Kundennummer eingeben: ");
						a = Console.ReadLine();
						id = CustomerWrapper.Intf_deleteCustomer(Int32.Parse(a));
						if (id >= 0)
							Console.WriteLine("Erfolgreich gelöscht.");
						else
							Console.WriteLine("Falsche Eingaben, bitte wiederholen.");
					}
					catch (Exception e)
					{
						Console.WriteLine("Error: " + e.Message);
					}
					return true;

				case "4":	//Konto erstellen
					try
					{
						Console.WriteLine("Um ein neues Konto zu erstellen, bitte folgende Daten mit Beistrich und Leerzeichen (, ) getrennt eingeben.");
						Console.WriteLine("Kontotyp(0 = Spar, 1 = Kredit), Anfangsstand, Name des Kontos.\nEingabe: ");
						a = Console.ReadLine();
						id = AccountWrapper.Intf_createAccount(Int32.Parse(a.Split(',', ' ')[0]), Double.Parse(a.Split(',', ' ')[2]), a.Split(',', ' ')[4]);
						if (id >= 0)
							Console.WriteLine("Erfolgreich erstellt. Kontonummer = " + id +", bitte merken.");
						else
							Console.WriteLine("Falsche Eingaben, bitte wiederholen.");
					}
					catch (Exception e)
					{
						Console.WriteLine("Error: " + e.Message);
					}
					return true;

				case "5":	//Konto bearbeiten
					try
					{
						Console.WriteLine("Um ein Konto zu bearbeiten, bitte folgende Daten mit Beistrich und Leerzeichen (, ) getrennt eingeben.");
						Console.WriteLine("Kontonummer, Kontotyp(0 = Spar, 1 = Kredit)\nEingabe: ");
						a = Console.ReadLine();
						id = AccountWrapper.Intf_editAccount(Int32.Parse(a.Split(',', ' ')[0]), Int32.Parse(a.Split(',', ' ')[2]));
						if (id >= 0)
							Console.WriteLine("Erfolgreich bearbeitet.");
						else
							Console.WriteLine("Falsche Eingaben, bitte wiederholen.");
					}
					catch (Exception e)
					{
						Console.WriteLine("Error: " + e.Message);
					}
					return true;

				case "6":	//Konto löschen
					try
					{
						Console.WriteLine("Um ein Konto zu löschen, bitte die Kontonummer eingeben: ");
						a = Console.ReadLine();
						id = AccountWrapper.Intf_deleteAccount(Int32.Parse(a));
						if (id >= 0)
							Console.WriteLine("Erfolgreich gelöscht.");
						else
							Console.WriteLine("Falsche Eingaben, bitte wiederholen.");
					}
					catch (Exception e)
					{
						Console.WriteLine("Error: " + e.Message);
					}
					return true;

				case "7":	//Kontoauszug erstellen
					try
					{
						Console.WriteLine("Um einen Kontoauszug zu erstellen, bitte die Kontonummer eingeben: ");
						a = Console.ReadLine();
						id = AccountWrapper.Intf_createBankStatement(Int32.Parse(a));
						if (id >= 0)
							Console.WriteLine("Erfolgreich erstellt.");
						else
							Console.WriteLine("Falsche Eingaben, bitte wiederholen.");
					}
					catch (Exception e)
					{
						Console.WriteLine("Error: " + e.Message);
					}
					return true;

				case "8":	//Neue Ueberweisung
					try
					{
						//Die Überweisung braucht auch einen zum Konto zugewiesenen Kunden.
						Console.WriteLine("Um eine neue Ueberweisung durchzuführen, bitte folgende Daten mit Beistrich und Leerzeichen (, ) getrennt eingeben.");
						Console.WriteLine("Kundenummer (der die Ueberweisung startet), Von Kontonummer, zu Kontonummer, Betrag, in Waehrung(EUR = 0, USD = 1, GBP = 2, INR = 3, JPY = 4)\nEingabe: ");
						a = Console.ReadLine();
						AccountWrapper.Intf_attachAccount(Int32.Parse(a.Split(',', ' ')[2]), Int32.Parse(a.Split(',', ' ')[0]));
						id = TransactionWrapper.Intf_transfer(Int32.Parse(a.Split(',', ' ')[0]), Int32.Parse(a.Split(',', ' ')[2]), Int32.Parse(a.Split(',', ' ')[4]), float.Parse(a.Split(',', ' ')[6]), Int32.Parse(a.Split(',', ' ')[8]));
						if (id >= 0)
							Console.WriteLine("Erfolgreich ueberwiesen.");
						else
							Console.WriteLine("Falsche Eingaben bei der Ueberweisung, bitte wiederholen.");
					}
					catch (Exception e)
					{
						Console.WriteLine("Error: " + e.Message);
					}
					return true;

				case "9":   //Neue Remote - Ueberweisung
					try
					{
						//Die Überweisung braucht auch einen zum Konto zugewiesenen Kunden.
						Console.WriteLine("Um eine neue Ueberweisung durchzuführen, bitte folgende Daten mit Beistrich und Leerzeichen (, ) getrennt eingeben.");
						Console.WriteLine("Von Kontonummer, von BIC, zu Kontonummer, zu BIC, Betrag, in Waehrung(EUR = 0, USD = 1, GBP = 2)\nEingabe: ");
						a = Console.ReadLine();

						string fromIban = a.Split(',', ' ')[0];
						string fromBic = a.Split(',', ' ')[2];
						string toIban = a.Split(',', ' ')[4];
						string toBic = a.Split(',', ' ')[6];
						double value = double.Parse(a.Split(',', ' ')[8]);
						ECurrency currency = ECurrency.Euro;
						if (a.Split(',', ' ')[10].Equals("1"))
							currency = ECurrency.Dollar;
						else if(a.Split(',', ' ')[10].Equals("2"))
							currency = ECurrency.Pound;
						RemoteTransaction transaction = new RemoteTransaction(RemoteTransaction.GenerateIbanFromAccID(fromIban), fromBic, RemoteTransaction.GenerateIbanFromAccID(toIban), toBic, value, currency);
						TransactionFunctions.Send(transaction);
                    }
					catch (Exception e)
					{
						Console.WriteLine("Error: " + e.Message);
					}
					return true;

				case "10":	//Einzahlen
					try
					{
						Console.WriteLine("Um etwas auf einem Konto einzuzahlen, bitte folgende Daten mit Beistrich und Leerzeichen (, ) getrennt eingeben.");
						Console.WriteLine("Kontonummer, Betrag\nEingabe: ");
						a = Console.ReadLine();
						id = TransactionWrapper.Intf_deposit(Int32.Parse(a.Split(',', ' ')[0]), float.Parse(a.Split(',', ' ')[2]));
						if (id >= 0)
							Console.WriteLine("Erfolgreich eingezahlt.");
						else
							Console.WriteLine("Falsche Eingaben, bitte wiederholen.");
					}
					catch (Exception e)
					{
						Console.WriteLine("Error: " + e.Message);
					}
					return true;

				case "11":   //Abheben
					try
					{
						Console.WriteLine("Um etwas von einem Konto abzuheben, bitte folgende Daten mit Beistrich und Leerzeichen (, ) getrennt eingeben.");
						Console.WriteLine("Kontonummer, Betrag\nEingabe: ");
						a = Console.ReadLine();
						id = TransactionWrapper.Intf_withdraw(Int32.Parse(a.Split(',', ' ')[0]), float.Parse(a.Split(',', ' ')[2]));
						if (id >= 0)
							Console.WriteLine("Erfolgreich abgehoben.");
						else
							Console.WriteLine("Falsche Eingaben, bitte wiederholen.");
					}
					catch (Exception e)
					{
						Console.WriteLine("Error: " + e.Message);
					}
					return true;

				case "12":   //Wartungsaufgaben
					try
					{
						Console.WriteLine("Räumt nun alle Daten auf. Alles wird gelöscht. Bitte mit \"ja\" bestätigen.");
						a = Console.ReadLine();
						if (a.Equals("ja"))
							DataMaintenance.Intf_clearData();

					}
					catch (Exception e)
					{
						Console.WriteLine("Error: " + e.Message);
					}
					return true;

				case "exit":
					return false;

				default:
					hilfe();
					return true;
			}
		}
    }
}
