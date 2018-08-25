let users = {
  createoredit: function (id, nick, email, password, confpassword, role, avatar) {
      if (password !== confpassword){
          dom.setError("Passwords are not equal...");
          return;
      } 
      let user = {
          user_id: id,
          user_nick:nick,
          user_email:email,
          user_password:password,
          user_confpassword:confpassword,
          user_role:role,
          user_avatar:avatar
      };
      
      http.post("/admin/account/createoredit", user, function (data, status) {
          
      },function (data, status) {
          dom.setError("Failed to proceed with request!");
      });
  },
    remove: function (id) {
        http.delete("/admin/account/remove/" + id, function (data, status) {
           location.reload(); 
        });
    }
};