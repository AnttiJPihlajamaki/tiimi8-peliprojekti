# Kameran Suunnitelu
## Mahdollisia eri toteutuksia kameraan
Pelimme on akvaariokauppasimulaattori. Peli tehdään 2D:nä ja sen konsepti on sen
verran yksinkertainen, että kameran toteutukseen on vain muutamia mahdollisuuksia.

### 1. Pelissä on eri näkymiä
Pelissä voi olla monta kameraa, joiden välillä vaihdellaan, sen perusteella,
mitä pelaaja haluaa pelissä tehdä Sopii paremminen, jos peli on jaettu moneen
eri sceneen.  
**Hyviä puolia**
 - Sopii hyvin Yeti-tabletille, kun akvaario näkymä täyttää koko näytön.
 - Pelin osia on helpompi jakaa toisistaan, jolloin voidaan keskittyä aluksi
kehittämään vain akvaarionäkymää.  
**Huonoja puolia**
- Jos näkymä on kokoajan paikallaan, se voi olla tylsä.
- Peliä ei kannata rakentaa moneen eri scenee, koska ne ovat liian
riippuvaisia toisistaan.
### 2. Pelissä on yksi kamera, jota voidaan zoomata
Pelissä on yksi kamera, jota pelaaja voi zoomata tarkastelemaan akvaarioita ja
muita pelin elemenntejä.  
**Hyviä puolia**
 - Sopii paremmin yhteen sceneen  
**Huonoja puolia**
 - Pelaajalla voi olla vaikeutta ymmärtää mihin työkaluja käytetään ja mihin
 pelin osaan pitäisi keskittyä.
 - Peli tulisi rakentaa lähtökohtaisesti sisälttyttämään akvaariokaupan.
## Kameran Alustava Päätös
Pelissä pääasiassa kaksi eri näkymää, akvaario näkymä ja akvaariokaupan näkymä.
### Akvaario Näkymä
Akvaariossa käytetään staattista kameraa, joka kuvaa koko akvaarion. Näkymään
voidaan myös lisätä zoomauksia eri tilanteissa ja pelaajan syötteellä.
### Akvaariokaupan Näkymä
Akvaariokaupasta näytetään koko pituus kerralla ja kameraa voidaa vetää sormella
vasemmalle ja oikealle.
