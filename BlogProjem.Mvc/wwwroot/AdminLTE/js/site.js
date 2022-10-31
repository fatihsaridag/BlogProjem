function convertFirstLetterToUpperCase(text) {                      // string yazı aldık.
    return text.charAt(0).toUpperCase() + text.slice(1);            //Baş harflerini büyük yapmmak için (True) (False)
 // bizim vereceğimiz indeksteki harfi aldı(ilk)  ve büyük harfe dönüştürdük. Bizim tüm cümleyi geri döndürmek istediğimiz için 1.indeksten itibaren cümleyi aldık.
}

function convertToShortDate(dateString) {                                       //bizler datestring aldık.
    const shortDate = new Date(dateString).toLocaleDateString('en-US');         //Tarih formatımız da short date olarak geldi
                            //date stringe çevirdik. ve local verdik(Tablomuzda kullanılan format)
    return shortDate;
}