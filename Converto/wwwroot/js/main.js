(function() {
    let docxToPdf = document.getElementById("DocxToPdf");
    docxToPdf.addEventListener("click", DocxToPdfEventHandler);
    function DocxToPdfEventHandler() {
        document.location.href = "convert/wordtopdf";
    }

    let docxToJpg = document.getElementById("DocxToJpg");
    docxToJpg.addEventListener("click", DocxToJpgEventHandler);
    function DocxToJpgEventHandler() {
        document.location.href = "convert/wordtojpg";
    }

    let docxToTxt = document.getElementById("DocxToTxt");
    docxToTxt.addEventListener("click", DocxToTxtEventHandler);
    function DocxToTxtEventHandler() {
        document.location.href = "convert/wordtotxt";
    }
})();