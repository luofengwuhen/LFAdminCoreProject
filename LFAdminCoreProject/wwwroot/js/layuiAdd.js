//检测本地是否存在access_token，如不存在，则跳转到登录页
function redirect(e) {
    var access_token = layui.data(layui.setter.tableName)['access_token'];
    if (access_token !== null)
        location.href = e;
    else
        location.href = '/Login/Index';
} 