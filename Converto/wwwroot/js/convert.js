(function () {
    //Managing file input
    let inputFiles = document.getElementById("inputFiles");
    inputFiles.addEventListener("change", inputChangeEventHandler);

    function inputChangeEventHandler() {
        let files = inputFiles.files;
        console.log(files);
        if (files.length > 3) {
            alert("You can not convert more than 3 files");
            return false;
        }

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

        let method = document.getElementsByClassName("convert__page-title")[0].id;
        Convert(method, formData);
    }
    function Convert(convertBackendMethod, formData) {
        $.ajax({
            type: "post",
            url: `/Convert/${convertBackendMethod}`,
            processData: false,
            contentType: false,
            data: formData
        }).done(function (partialView) {
            $(".conversion__process").remove();
            $(".convert__page-subtitle").after(partialView);
        });
    }

    //Drag and drop
    let dropZone = document.getElementById("convertForm");
    dropZone.addEventListener("dragover", function (e) {
        e.preventDefault();
    });
    dropZone.addEventListener("drop", function (e) {
        e.preventDefault();
        let files = e.dataTransfer.files;
        inputFiles.files = files;
        inputChangeEventHandler();
    });
})();