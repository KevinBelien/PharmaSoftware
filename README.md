# PharmaSoftware

link naar github: https://github.com/KevinBelien/PharmaSoftware
naam github-project: PharmaSoftware

Gebruikersnamen en wachtwoorden:
Gebruikersnaam: Test1234    Wachtwoord: Test1234

Beschrijving: 
Applicatie gemaakt voor een groep van apothekers (die samenwerken) om hun stock bij te houden.
Applicatie is bedoeld om later aan kassasysteem te connecteren.

Notes:
1. Als er te weinig van éénzelfde product in stock is, zal de apotheker dit kunnen opmerken aan de sidebar van bijna elke pagina als men is ingelogd.
Een stock issue is een issue als men aan al deze vereisten voldoet:
  - Het "aantal in stock" kleiner is als 10.
  - Het "aantal besteld" kleiner is als 20.
  
Dit wilt dus ook zeggen dat in de datagrid bij voorraad, het veld "stock" rood kan kleuren. 
Terwijl er geen stock issue is. Dit komt doordat het artikel reeds besteld is, maar nog niet is geleverd.

2. Een product is equal als het product van type product is + de code identiek is.

3. Dit leg ik uit a.d.v een voorbeeld:
  Als een apotheker al een product in stock heeft met een 13-cijferige code "129..." en een andere apotheker een product wilt toevoegen met dezelfde code.
  Dan zal het product worden toegevoegd (in de database) aan de tussenentiteit van die apotheker zonder dat deze nog eens toegevoegd wordt aan de entiteit "product" (in de database).

  Als de code identiek is, maar de naam bijvoorbeeld niet van het product. Dan zal de naam automatisch aangepast worden aan de originele staat voor deze apotheker.
  
4. Producten die verwijderd worden door een apotheker zullen alleen definitief verwijderd worden (in de entiteit product), als geen enkele apotheker van de groep dit product in stock heeft/besteld heeft.

5. De apotheker moet zijn stock nog altijd manueel controleren doordat dit niet aan andere systemen gelinkt is. Men kan producten verwijderen uit zijn stock en de aantallen in stock + aantallen besteld wijzigen. 

6. Als men een product wilt toevoegen en een subcategorie wilt toewijzen aan dit product, zal men eerst een categorie moeten toewijzen. dan wordt dit veld enabled.

7. Als het bestelde product geleverd wordt en de stock dus wordt gewijzigd naar meer en het "aantal besteld" naar 0. Dan zal de property "last order" onveranderd blijven. 

Opsomming van opzoekingswerk:
1. Dependencyproperties: gebruikt in navigatiebuttons in de map customcontrols.

2. Bindable PasswordBox: customControls => BindablePasswordBox.

3. MVVM Light Relaycommands: Hierdoor kon ik views sluiten als er een andere geopend werd.

4. Converters: 3 x gebruikt. in datagrid van voorraad om ervoor te zorgen dat als men op de expand knop drukt, de RowDetailTemplate "visible" te maken.

5. <i:Interaction.Triggers> (interactivity): mogelijkheid om events toe te voegen en methode te koppelen: gebruikt om bv producten te filteren wanneer text veranderd.

6. Hyperlinks

7. FindAncestor + Ancestortype bij relativesource. De mogelijkheid om binnen een bepaalde parent de binden. (veel gebruikt)

8. Stringformats en converterculture in bindings van view.xaml. (gebruikt datagridColumn: laatste order)

9. Checkbox in datagrid.


