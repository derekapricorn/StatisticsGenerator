import string 
def read_file():
    fname = "TotalTemp.txt" #opens file with name of "test.txt"
    
    with open(fname) as f:
        content = f.readlines()
        for item in content:
            str_temp =  string.replace(item, " ", "\t")
            ff = open('newFile.text', 'a')
            ff.write(str_temp)
            ff.close()
##        strArr = content.split()
##        for s in strArr:
##            print s
##        ff = open('newFile.txt', 'a')
##        ff.write('\n' + content
        
    

read_file()
