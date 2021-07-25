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
    }
}