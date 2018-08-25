let modules = {
    'createoredit': function (id, name, active, js) {
        let module = {
            module_id: id,
            module_name: name,
            module_js: js,
            module_active: active
        };
        http.post("/admin/modules/createoredit",module, function (data, status) {
            if (status === 200){
                location.reload();
            }
            else{
                dom.setError("Something went wrong during the request!")
            }
        });
    },
    'remove': function (id) {
        http.delete("/admin/modules/delete/" + id, null, function (data, status) {
            if (status === 200){
                location.reload();
            }
            else{
                dom.setError("Something went wrong during the request!")
            }
        });
    }

};
