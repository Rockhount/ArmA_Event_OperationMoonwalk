# ArmA 3 Event Mission "Operation Moonwalk"

# Support
Es wird kein Support angeboten. Anfragen werden ignoriert.

# Pull Requests
Pull Requests werden ignoriert.

## Inhalt
Hier finden sich alle Infos, Benchmarks und der gesamte Source-Code, der in der PBO und DLL's steckt.

## Zweck
Dieses Material wird der Community zur Verfügung gestellt, damit man sehen kann was möglich ist und damit man es auch für die eigenen Zwecke verwenden kann. Es ist nicht für den breiten Einsatz gedacht und es soll auch kein Framework werden, dass man leicht implementieren können soll, da durch den daraus entstehenden Overhead zu viel Performance flöten geht. Es geht nur um die Performance und ArmA durch die DLL's so viel Arbeit und Traffic wie nur möglich zu nehmen, damit die Effizienz steigt. Dementsprechend sieht natürlich auch der Code aus. Das soll also nichts für Anfänger sein.

## Performance-Resultate
http://46.4.100.232/forums/topic/10-02-2018-arma-3-coop46-operation-moonwalk/page/5/#post-3779

## Zielgruppe
In diesem Fall werden fortgeschrittene SQF's, FSM's und C# .NET DLL's eingesetzt, die nichts für Anfänger sind. Nichtsdestotrotz kann man anhand der Tabellen-Grafiken leicht erkennen, welche Resultate sich aus der Technik ergeben.

## Umgebung
Die Skripte wurden mit Notepad++ und dem BI-FSM Editor erstellt. Die DLL's wurden mit Visual Studio 2015 erstellt.

## Umfang
Die Anzahl der in den DLL's und Skripten eingesetzten Codezeilen beträgt ~7000.

## Geschichte
Ich hätte mir das hier selbst vor jahren gewünscht, weshalb ich es nun an andere weitergebe. Die Geschichte zu dieser fortgeschrittenen Technik fing im Dezember 2014 an. Das war die Zeit als die großen Communitys die Events noch für ArmA 2 veranstalteten. Es gab noch zu wenig Mods für ArmA 3 und wenn, dann waren sie zu instabil. Doch dank Patch 1.59 für ArmA 2 gab es zumindest die richtigen Skript-Befehle, um mit der Implementierung der Technik anzufangen, die man in vollendeter Form heute hier betrachten kann. Es ist kaum 2 Jahre her, aber damals war der Einsatz von selbstgeschriebenen DLL's noch Neuland und kaum einer kannte sich damit aus. Und so fing ich zunächst damit an die DLL's als Ersatz für Skriptfunktionen als auch später für die Cache-Berechnungen zu nutzen. Heute übernehmen die DLL's zusätzlich die Kommunikation(HC<->Server<->Client) für die großen Mengen an Netzwerkdaten, die für das Cachen anfallen. Die Skripte, DLL's und das Wissen entwickelten sich mit den Jahren immer weiter, wodurch die Stabilität und vor allem die Performance heute an einem Punkt gelangt sind, an dem keine große Fortschritte mehr zu erzielen sind. Doch die Grenzen wurden noch nicht ausgelotet und die Coop 100 will immernoch bezwungen werden.