(function () {
    //Managing file input
    let inputFiles = document.getElementById("inputFiles");
    inputFiles.addEventListener("change", inputChangeEventHandler);

    function inputChangeEventHandler() {
        let files = this.files;
        let formData = new FormData();
        for (let i = 0; i < files.length; i++) {
            formData.append("files", files[i]);
        }

        $.ajax({
            url: "/Convert/WordFilesInfo",
            type: "post",
            processData: false,
            contentType: false,
            data: formData
        }).done(function (partialView) {
            let form = document.getElementById("convertForm");
            form.insertAdjacentHTML("beforeend", partialView);
        });
    }


    //Form submit ajax
    let convertForm = document.getElementById("convertForm");
    convertForm.addEventListener("submit", formSubmitEventHandler);

    
    function formSubmitEventHandler(e) {
        e.preventDefault();
        let inputFiles = document.getElementById("inputFiles");
        let files = inputFiles.files;

        let formData = new FormData();
        for (let i = 0; i < files.length; i++) {
            formData.append("files", files[i]);
        }

        $.ajax({
            type: "post",
            url: "/Convert/WordToPdf",
            processData: false,
            contentType: false,
            data: formData
        }).done(function (partialView) {
            console.log(formData);
        });
    }

})();