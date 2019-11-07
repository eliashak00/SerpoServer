let modules = {
    'createoredit': function(id, name, js) {
        let module = {
            module_id: id,
            module_name: name,
            module_js: js,
        };
        http.post("/admin/modules/createoredit",
            module,
            function(data, status) {
                if (status === 200) {
                    window.location = "/admin/modules/";
                } else {
                    dom.setError("Something went wrong during the request!");
                }
            });
    },
    'remove': function(id) {
        http.delete("/admin/modules/delete/" + id,
            null,
            function(data, status) {
                if (status === 200) {
                    location.reload();
                } else {
                    dom.setError("Something went wrong during the request!");
                }
            });
    }

};