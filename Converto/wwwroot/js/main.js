(function() {
    //Docx to Pdf
    let docxToPdf = document.getElementById("DocxToPdf");
    docxToPdf.addEventListener("click", DocxToPdfEventHandler);
    function DocxToPdfEventHandler(e) {
        document.location.href = "convert/wordtopdf";
    }
})();