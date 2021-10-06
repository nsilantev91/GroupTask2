import numpy as np
import matplotlib.pyplot as plt
import matplotlib.image as mpimg
import operator
from collections import Counter
from PIL import Image

#First formula
def rgb_to_gray(img):
        grayImage = np.zeros(img.shape)
        R = np.array(img[:, :, 0])
        G = np.array(img[:, :, 1])
        B = np.array(img[:, :, 2])

        R = (R *.2989)
        G = (G *.5870)
        B = (B *.1140)

        Avg = (R+G+B)
        grayImage = img.copy()

        for i in range(3):
           grayImage[:,:,i] = Avg
           
        return grayImage       

#Second formula
def rgb_to_gray1(img):
        grayImage = np.zeros(img.shape)
        R = np.array(img[:, :, 0])
        G = np.array(img[:, :, 1])
        B = np.array(img[:, :, 2])

        R = (R *.2226)
        G = (G *.7152)
        B = (B *.0722)

        Avg = (R+G+B)
        grayImage = img.copy()

        for i in range(3):
           grayImage[:,:,i] = Avg
           
        return grayImage

#Image import
img = mpimg.imread('cat.jpg')

#Evaluating image according to first formula
gray = rgb_to_gray(img)
pixels_1 = []
for i in range(len(gray)):
    for j in range(len(gray[i])):
        pixels_1.append(gray[i][j][0])
ctr = Counter(pixels_1)

#Evaluting image according to second formula
gray2 = rgb_to_gray1(img)
pixels_2 = []
for i in range(len(gray2)):
    for j in range(len(gray2[i])):
        pixels_2.append(gray2[i][j][0])

pixels_3 = []
#First and second image diffrence
tt = np.asarray(gray).astype(int)
tt2 = np.asarray(gray2).astype(int)
temp = tt - tt2
temp = np.absolute(temp)
temp = temp.astype(np.uint8)*10
diff = Image.fromarray(temp)
for i in range(len(temp)):
    for j in range(len(temp[i])):
        pixels_3.append(temp[i][j][0])

ctr2 = Counter(pixels_3)
print(type(temp[1][1][1]))

#diff = ImageChops.difference(gray,gray2)

#Printing out results
f, arr = plt.subplots(3,2)
arr[0][0].imshow(gray, cmap='gray', vmin=0, vmax=255)
arr[0][1].imshow(gray2, cmap=plt.get_cmap('gray'), vmin=0, vmax=255)
arr[1][0].bar(ctr.keys(),ctr.values())
ctr = Counter(pixels_2)
arr[1][1].bar(ctr.keys(),ctr.values())
arr[2][0].imshow(diff, cmap=plt.get_cmap('gray'), vmin=0, vmax=255)
arr[2][1].bar(ctr2.keys(),ctr2.values())
#diff.show()
plt.show()




