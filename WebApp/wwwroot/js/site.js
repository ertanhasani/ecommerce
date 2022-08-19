$(document).ready(function () {

    const persianToEnglishNumber = function (strNum) {
        const pn = ["۰", "۱", "۲", "۳", "۴", "۵", "۶", "۷", "۸", "۹"];
        const en = ["0", "1", "2", "3", "4", "5", "6", "7", "8", "9"];
        const an = ["٠", "١", "٢", "٣", "٤", "٥", "٦", "٧", "٨", "٩"];
        let cache = strNum;
        for (let i = 0; i < 10; i++) {
            let regex_fa = new RegExp(pn[i], 'g');
            let regex_ar = new RegExp(an[i], 'g');
            cache = cache.replace(regex_fa, en[i]);
            cache = cache.replace(regex_ar, en[i]);
        }
        return cache;
    };

    const englishToPersianNumber = function (strNum) {
        const en = ["۰", "۱", "۲", "۳", "۴", "۵", "۶", "۷", "۸", "۹"];
        const pn = ["0", "1", "2", "3", "4", "5", "6", "7", "8", "9"];
        const an = ["٠", "١", "٢", "٣", "٤", "٥", "٦", "٧", "٨", "٩"];
        let cache = strNum;
        for (let i = 0; i < 10; i++) {
            let regex_fa = new RegExp(pn[i], 'g');
            let regex_ar = new RegExp(an[i], 'g');
            cache = cache.replace(regex_fa, en[i]);
            cache = cache.replace(regex_ar, en[i]);
        }
        return cache;
    };

    const priceToPersian = function () {

        const p = $('p');

        for (let index = 0; index < p.length; ++index) {
            const element = p[index];
            element.innerHTML = englishToPersianNumber(element.innerHTML)
        }

        const label = $('label');

        for (let index = 0; index < label.length; ++index) {
            const element = label[index];
            element.innerHTML = englishToPersianNumber(element.innerHTML)
        }

        let h = $(":header");

        for (let index = 0; index < h.length; ++index) {
            const element = h[index];
            element.innerHTML = englishToPersianNumber(element.innerHTML)
        }

        const footer = $("footer");

        for (let index = 0; index < footer.length; ++index) {
            const element = footer[index];
            element.innerHTML = englishToPersianNumber(element.innerHTML)
        }

        const price = $(".price");

        for (let index = 0; index < price.length; ++index) {
            const element = price[index];
            element.innerHTML = englishToPersianNumber(element.innerHTML)
        }
    };

    priceToPersian();

    $('.persianDatepicker').persianDatepicker({
        initialValue: false,
        format: 'YYYY/MM/DD',
        observer: false,
        calendar: {
            persian: {
                locale: 'en',
            }
        }
    });
})