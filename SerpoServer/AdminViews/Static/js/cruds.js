let cruds = {
    'createoredit': function (id, route, name, struct) {
        let crud = {
            crud_id: id,
            crud_route: route,
            crud_table: name,
            crud_struct: String(struct)
        };
        http.post("/admin/pages/createoredit?type=crud", crud, function (data, status) {
            if (status === 200){
                window.location = "/admin/pages?type=crud";
            }
            else{
                dom.setError("Something went wrong during the request!")
                
            }
        });
    },
    'remove': function (id) {
        http.delete("/admin/pages/delete/" + id + "?type=crud", null, function (data, status) {
            if (status === 200){
                location.reload();
            }
            else{
                dom.setError("Something went wrong during the request!")
            }
        });
    }

};
