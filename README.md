# NOTINOhw
## Problémy s původním kódem
- Nemožnost výběru vstupních souborů -> vstup uživatele
- Document třída do vlastního souboru
- Proměnná input je použita mimo blok, kde je vytvořena -> deklarace před try-catch
- StreamWriter implementuje IDisposable, ale není použit v usingu -> Zabalit do using bloku
- Pokud neexistuje cesta k výstupnímu souboru, vyhodí exception -> Zkontrolovat cestu a vytvořit složky pokud neexistují
- Proměnné jsou typované pomocí var -> přepsat přímo na typy (Může být subjektivní, ale myslím si že je kód lépe čitelný, když jsou typy jasně dány "hned ze začátku")
