var List = {
    Delete: function (ID) {
        document.getElementById("modelID").value = ID;
    },
};

var Post = {
    DeleteBanner: function (ID) {
        $.ajax({
            type: 'post',
            url: '/post/delbanner/',
            data: { ID }
        }).done((data) => {

            if (data.success) {
                $("#delBanner").remove();
                $("#banner").remove();
            }
        });
    },
    DeletePicture: function (ID) {
        $.ajax({
            type: 'post',
            url: '/user/delpicture/',
            data: { ID }
        }).done((data) => {

            if (data.success) {
                $("#delPicture").remove();
                $("#picture").remove();
            }
        });
    }
}