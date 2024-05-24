var btn = document.querySelectorAll(".item-delete")
btn.forEach(button => {
    button.addEventListener("click", function (e) {
        e.preventDefault()
        Swal.fire({
            title: "Are you sure?",
            text: "You won't be able to revert this!",
            icon: "warning",
            showCancelButton: true,
            confirmButtonColor: "#3085d6",
            cancelButtonColor: "#d33",
            confirmButtonText: "Yes, delete it!"
        }).then((result) => {
            if (result.isConfirmed) {
                var url = button.getAttribute("href");
                fetch(url)
                    .then(resp => {
                        if (resp.status == 200) {
                            Swal.fire({
                                title: "Deleted!",
                                text: "Your file has been deleted.",
                                icon: "success"
                            });
                            button.parentElement.parentElement.remove()
                        }
                        else {
                            Swal.fire({
                                title: "silinmir",
                                text: "Your file has been deleted.",
                                icon: "error"
                            });

                        }
                    })

            }
        });
    })
})
