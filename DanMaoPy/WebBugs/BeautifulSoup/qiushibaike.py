# -*- coding:utf-8 -*-
import urllib
from urllib import  request

page = 1
url = 'http://www.qiushibaike.com/hot/page/' + str(page)
user_agent = 'Mozilla/4.0 (compatible; MSIE 5.5; Windows NT)'
headers = { 'User-Agent' : user_agent }
request = urllib.request.Request(url=url,headers = headers)
response = urllib.request.urlopen(request)
print (response.read())

#req = urllib.request.Request(url=url_address, headers=headers)
#    return urllib.request.urlopen(req)