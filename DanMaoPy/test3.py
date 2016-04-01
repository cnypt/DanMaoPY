# -*- coding: utf-8 -*-
name=input('你叫什么名字：')
height=input('你的体重是：')
weight=input('你的身高是：')
height=float(height)
weight=float(weight)
bmi=height/(weight*weight)
print(name,'的BMI是：%.2f' %bmi)
if bmi < 18.8:
	print('过轻')
elif bmi < 25:
	print('正常')
elif 28<=bmi<=32:
	print('肥胖')
else:
	print('严重过胖')