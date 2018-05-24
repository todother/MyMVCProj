
// pages/mine/mine.js
Page({

  /**
   * 页面的初始数据
   */
  data: {
    imgs:[],
    rowsCounts:0,
    delOrShow:0,
    uInfo:[],
    sessionk:"",
    openId:""
  },

  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {
    var that=this;
    wx.login({
      success: res => {
        if (res.code) {
          //发起网络请求
          wx.request({
            url: 'http://localhost:8888/user/postWebservice',
            data: {
              code: res.code
            },
            success:function(res){
              that.setData({
                sessionK:res.data.session_key,
                openId:res.data.openid
              });
              wx.getSetting({
                success: res => {
                  if (res.authSetting['scope.userInfo']) {
                    // 已经授权，可以直接调用 getUserInfo 获取头像昵称，不会弹框
                    wx.getUserInfo({
                      withCredentials: true,
                      success: res => {
                        // 可以将 res 发送给后台解码出 unionId
                        that.setData({
                          uinfo: res.userInfo
                        });
                        wx.request({
                          url: 'http://localhost:8888/user/wxDecryptData',
                          data: {
                            sessionKey: that.data.sessionK,
                            encryptedData: that.data.uinfo.encryptedData,
                            iv: that.data.uinfo.iv
                          },
                          success: function (res) {
                            wx.showModal({
                              title: 'Result',
                              content: res.data,
                              showCancel: true,
                              cancelText: '2',
                              cancelColor: '',
                              confirmText: '1',
                              confirmColor: '',
                              success: function (res) { },
                              fail: function (res) { },
                              complete: function (res) { },
                            })
                          }
                        })

                      }
                    })
                  } else {
                    console.log('登录失败！' + res.errMsg)
                  }
                }
              })
                        // 由于 getUserInfo 是网络请求，可能会在 Page.onLoad 之后才返回
                        // 所以此处加入 callback 以防止这种情况
                        if (this.userInfoReadyCallback) {
                          this.userInfoReadyCallback(res)
                        }
                      }
                    })
                  }
                }
              });
              
    // 获取用户信息
    
  },
  getLoggedUser:function(){
    wx.getUserInfo({
      withCredentials:true,
      success:function(res){
        console.log(res.encryptedData);
        console.log(res.iv);
        console.log(res.signature);
        console.log(res.userInfo);
      }
    })
  },
  /**
   * 生命周期函数--监听页面初次渲染完成
   */
  onReady: function () {
  
  },

  /**
   * 生命周期函数--监听页面显示
   */
  onShow: function () {
    
  },

  /**
   * 生命周期函数--监听页面隐藏
   */
  onHide: function () {
  
  },

  /**
   * 生命周期函数--监听页面卸载
   */
  onUnload: function () {
  
  },

  /**
   * 页面相关事件处理函数--监听用户下拉动作
   */
  onPullDownRefresh: function () {
  
  },

  /**
   * 页面上拉触底事件的处理函数
   */
  onReachBottom: function () {
  
  },

  /**
   * 用户点击右上角分享
   */
  onShareAppMessage: function () {
  
  },
  showImg:function(e){
    if(this.data.delOrShow==0){
    var current=e.target.dataset.src;
    wx.previewImage({
      current: current,
      urls: this.data.imgs,
      success:function(res){
        wx.showModal({
          title: 'title',
          content: '123',
          showCancel: true,
          cancelText: 'Cancel',
          cancelColor: '',
          confirmText: 'Confirm',
          confirmColor: '',
          success: function(res) {},
          fail: function(res) {},
          complete: function(res) {},
        })
      },
      fail:function(res){
        var i =1;
        console.log(res.errorMessage);
      }
    })}
    else{
      this.setData({delOrShow:0});
    }
  },
  chooseImg:function(){
    var that=this;
    
    wx.chooseImage({
      success: function(res) {
        that.setData({
          imgs:res.tempFilePaths
        });
      },
    })
  },
  picOrPhoto:function(){
    
  },
  removeAt:function(e)
  {
  this.setData({delOrShow:1});
   delete this.data.imgs[e.target.dataset.id];
   var obj =this.data.imgs;
   this.setData({imgs:obj});
  },
  formSubmit:function(e){
    var filepath=this.data.imgs;
    var content=e.detail.value;
    wx.request({
      url: 'http://localhost:8888/user/generateAppid',
      data:{
        content:content
      },
      success:function(res){
        var appid=res.data.result;
        wx.showModal({
          title: 'result',
          content: appid,
        })
        wx.uploadFile({
          url: 'http://localhost:8888/user/savePic',
          filePath: filepath[0],
          name: 'name',
          formData:{
            appid:appid
          },
          success:function(res){
            console.log(res.data.result);
          }
        })
      }
    })
  }
})