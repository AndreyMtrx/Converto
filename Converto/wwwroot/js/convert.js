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
            url: "/Convert/FilesInfo",
            type: "post",
            processData: false,
            contentType: false,
            data: formData
        }).done(function (partialView) {
            $("#filesInfo").remove();
            let form = document.getElementById("convertForm");
            form.insertAdjacentHTML("beforeend", partialView);
            $("#filesInfo").slideDown(100);
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
            type: "get",
            url: "/Convert/ConversionProcess"
        }).done(function (partialView) {
            $("#filesInfo").slideUp(100, "linear", function () {
                convertForm.remove();
                $(".convert__page-subtitle").after(partialView);
            });

        });

        $.ajax({
            type: "post",
            url: "/Convert/WordToPdf",
            processData: false,
            contentType: false,
            data: formData
        }).done(function (partialView) {
            $(".conversion__process").remove();
            $(".convert__page-subtitle").after(partialView);
        });
    }

})();