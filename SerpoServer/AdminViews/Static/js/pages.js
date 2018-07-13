let pages = {
  'createoredit': function (id, route, script, site, view, methods, response) {
      let page = {
          page_id: id,
          page_route: route,
          page_script: script,
          page_site: site,
          page_view: view,
          page_methods: methods,
          page_response: response,
      };
      http.post("/admin/pages/createoredit", page, function (data, status) {
          if (status === 200){
              location.reload();
          }
          else{
              dom.setError("Something went wrong during the request!")
          }
      });
  },
    'remove': function (id) {
        http.delete("/admin/pages/delete/" + id, null, function (data, status) {
            if (status === 200){
              location.reload();
            }
            else{
              dom.setError("Something went wrong during the request!")
            }
        });
    }
    
};