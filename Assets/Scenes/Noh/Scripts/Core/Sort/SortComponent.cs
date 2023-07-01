using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.InputSystem;
/*
     ����� ������ �Ű�����Ÿ�Ը� �޶� �ϳ��� ����;����� IEnumerableŸ���� ����ȰŶ� ó���Ϸ��������� readonly �� �����̾ȵǳ� ... ����.
     ���ı�ɸ� �����Ұ��̱⶧����static ���� ���� ����� �Ҽ�����.    
     enum �� ����̱⶧���� �������̽��� ����Ҽ����� ���׸����� �������������. �׷��ٰ� ���׸��� Ŭ�����ι����� ��Ӷ����� �������߻��Ѵ�.
*/
/// <summary>
///     
///     Ŭ���� ���� ��(�ɹ����� or ������Ƽ)�� �������� ��ü �����ϰ������ ����� Ŭ�����̴�
///     ����� ������ ���۳�Ʈ�� ISortBase �� ��ӹ޾Ƽ� ���κ����� �ۼ��Ѵ�.
///     SortComponent<T>.SorttingDataArray(T[],SortComponent<T>.SortType); 
///     T�� ���۳�Ʈ �� �Ű������δ� T�� �迭�� �ѱ�� SortType �� ���Ĺ���̶� �Է¾��ص� ������� �⺻�� �������ķ� ����.
///     
///     enum SortType �� �߰��Ѱ����� �׽�Ʈ�Ϸ�
/// </summary>
/// <typeparam name="T">ISortBase �������̽��� ��ӹ��� Ŭ����</typeparam>

public static class SortComponent<T> where T : ISortBase 
{
    /// <summary>
    /// ���� �˰����� ����
    /// ������ ���ʿ��ϴ�.
    /// </summary>
    public enum SortProcessType
    { 
        InsertSort = 0, //���� �����ϱ� 
        SelectionSort,  //���� �����ϱ�
        BubbleSort,     //���� �����ϱ�
    }
    /// <summary>
    /// ���Ĺ�� ��������(ASCENDING), ��������(DESCENDING)
    /// </summary>
    public enum SortType 
    {
        Ascending = 0, //��������
        Descending     //��������
    }
    /// <summary>
    /// ���� �˰����� �������� �������ִ� �Լ�
    /// ��Ե� �ϳ��� ����;��µ� �ȵǳ� .. �����ε��ۿ� ���̾ȳ��´�..����
    /// �б⸸�ҰŸ� IEnumerable �� ���ں����� �����Ͽ� ����Ʈ�� �迭�̵� �Ѵٹ޾Ƽ� ó�������ϴ�.
    /// as �� �ڷ����� ��ȯ�� �����ϸ� ��ȭ��Ű�°��̶� ����Ÿ���� �������·� �̷������.
    /// �˰��� ��ó : https://hyo-ue4study.tistory.com/68
    ///<param name="data">������ ������</param>
    ///<param name="proccessType">������ �˰���Ÿ��</param>
    ///<param name="type">���Ĺ��</param>
    /// </summary>
    public static void SorttingData(IEnumerable<T> data,                                        //����Ʈ�� �迭�̵� �������ִ� �������̽��� ���ڰ����� �޾Ƽ� ó���غ��Ҵ�. 
                                    SortProcessType proccessType = SortProcessType.InsertSort,  //���ľ˰����� �������� �����ϱ����� �־����.
                                    SortType type =  SortType.Ascending                         //�������� �������� �����ϱ����� ���̴�.
                                    ) 
    {
        int length = data.Count();
        bool isArrayData = data is T[]; //�迭�� ��ȯ �������� üũ
        bool isListData = data is List<T>; //����Ʈ�� ��ȯ �������� üũ
        //�����ϳ��� true���̵ɰ��̴� �Ѵ� true �� �Ǵ����� ����.
        if (!isArrayData && !isListData) //�Ѵٺ�ȯ�̾ȵǸ� �ش� Ŭ�������� ó���ȵǴ� ��������
        {
            Debug.LogWarning("������ �ڷ����� �߸�����ϴ� ����ƮȤ�� �迭�� �����մϴ�");
            return;
        }
        switch (proccessType) // ���Ĺ������
        {
            case SortProcessType.InsertSort: //��������
                if (isArrayData) InsertionSort(data as T[], length ,type); //�迭
                else if (isListData) InsertionSort(data as List<T>, length, type); //����Ʈ
                break;
            case SortProcessType.SelectionSort: //��������
                if (isArrayData) SelectionSort(data as T[], length, type); //�迭
                else if (isListData) SelectionSort(data as List<T>, length, type);//����Ʈ
                break;
            case SortProcessType.BubbleSort: //��������
                if (isArrayData) BubbleSort(data as T[], length, type);//�迭
                else if (isListData) BubbleSort(data as List<T>, length, type);//����Ʈ
                break;
        }

    }

    /// <summary>
    /// �������� �˰����� �����ͼ� ����ߴ�\
    /// ���������� ���� ���� �������� ����(�ε����� �������� ����)���� ���Ľ�Ű�°��̴�.
    /// ���ʰ� �񱳸��ϸ鼭 �ϳ��� �����ϱ⶧���� j������ Ƚ���� �ڷΰ����� Ŀ����.
    /// ���Ҵ���̾������ break; �� ���������⶧���� ���ʿ��� �񱳴� �ִ������� ���δ�..
    /// </summary>
    /// <param name="arrayData">�迭���</param>
    /// <param name="length">�迭�� ũ��</param>
    private static void InsertionSort(T[] arrayData, int length, SortType type)
    {

        int j = 0;
        T key; //������ ��ü
        for (int i = 1; i < length; i++) //1��°���� �迭�� ũ�⸸ŭ ������ ���� 
        {
            key = arrayData[i];// 0���̾ƴ϶� 1������ ���� ��ü�� ��� 
            for (j = i - 1; j >= 0; j--) //���� ���� -1��°���� �񱳸� �����Ͽ� 
            {
                //Debug.Log(j);
                if (SortAscDesCheck(key, arrayData[j], type)) //���������϶�  key ������ ū���� �����Ұ��  ���������϶� key������ �������� �����Ұ��
                {
                    arrayData[j + 1] = arrayData[j];  // ��ĭ�� ��(+)�� �̵���Ų��.
                }
                else //�̹����ĵȰ��߿� ���� ��ü���� ������ ��ġ�� ���̻� �̵����ʿ䰡���⶧���� j������ ����������.
                {
                    break;
                }
            }
            arrayData[j + 1] = key; //  j������ ��ġ���� �����ͼ� ���������Ѱ��� �ٷεڿ� ���Ұ�ü�� �����͸� ��Ƽ� i������ �ϳ���������������. 
        }

    }
    /// <summary>
    /// �����ε��̶� ������ ����.
    /// </summary>
    /// <param name="listData">����Ʈ���</param>
    /// <param name="length">����Ʈ�� ũ��</param>
    private static void InsertionSort(List<T> listData, int length, SortType type)
    {

        int j = 0;
        T key; //������ ��ü
        for (int i = 1; i < length; i++) //1��°���� �迭�� ũ�⸸ŭ ������ ���� 
        {
            key = listData[i];// 0���̾ƴ϶� 1������ ���� ��ü�� ��� 
            for (j = i - 1; j >= 0; j--) //���� ���� -1��°���� �񱳸� �����Ͽ� 
            {
                //Debug.Log(j);
                if (SortAscDesCheck(key, listData[j], type)) //���������϶�  key ������ ū���� �����Ұ��  ���������϶� key������ �������� �����Ұ��
                {
                    listData[j + 1] = listData[j];  // ��ĭ�� ��(+)�� �̵���Ų��.
                }
                else //�̹����ĵȰ��߿� ���� ��ü���� ������ ��ġ�� ���̻� �̵����ʿ䰡���⶧���� j������ ����������.
                {
                    break;
                }
            }
            listData[j + 1] = key; //  j������ ��ġ���� �����ͼ� ���������Ѱ��� �ٷεڿ� ���Ұ�ü�� �����͸� ��Ƽ� i������ �ϳ���������������. 
        }

    }
    /// <summary>
    /// �������� �� ���������� ������ üũ���ִ� �Լ�
    /// </summary>
    /// <param name="min">�۾ƾߵɰ�</param>
    /// <param name="max">Ŀ�ߵ� ��</param>
    /// <param name="sortType">�������� �����������ð�</param>
    /// <returns></returns>
    private static bool SortAscDesCheck(T min , T max , SortType sortType) {
        bool resultValue = false;
        switch (sortType) {
            case SortType.Ascending:
                resultValue = min.SortValue < max.SortValue; // �ø������ϰ�� -100 ~ 100
                break;
            case SortType.Descending:
                resultValue = min.SortValue > max.SortValue; // ���������ϰ�� 100 ~ -100
                break;
        }
        return resultValue;
    }

    
    /// <summary>
    /// �������� �������İ��� �ݴ�� +�������� �񱳸��ϱ⶧���� 
    /// ó�� ���Ҷ� j������ Ƚ���� ������.
    /// </summary>
    /// <param name="arrayData">�迭���</param>
    /// <param name="length">�迭�� ũ��</param>
    private static void SelectionSort(T[] arrayData, int length, SortType type)
    {
        int i = 0;
        int j = 0;
        int min = 0;
        for (i = 0; i < length - 1; i++) // �迭ũ�⸸ŭ ���������� ������
        {
            min = i; //�ּҰ��� ã������ ��ġ���� �����Ѵ�
            for (j = i + 1; j < length; j++) //-������ �ּҰ������̳������̴� +������ Ž���� �����Ѵ�.
            {
                if (SortAscDesCheck(arrayData[j], arrayData[min],type))// min���� ���ؼ� min���� �������� �ִ°�� 
                {
                    min = j;//�ּҰ���ġ�� �����Ѵ�.
                }//�ݺ��Ͽ� ������ü�� �ּҰ���ġ�� ã�´�.
            }
            if (i != min)
            { 
                //�ּҰ��� ���� ���� ������������ ��ü�Ѵ�.
                //�Լ��ξȻ������� �Լ��λ��¼��� T[i] ���� �Ű������� �ѱ涧 ����Ÿ���� �ƴ϶� ��Ÿ������ ġȯ�� �Ǽ� �Ѿ�⶧���̴�.�����¸𸣰ٴ�.
                T tempObj = arrayData[i];
                arrayData[i] = arrayData[min];
                arrayData[min] = tempObj;
            }
        }
    }
    /// <summary>
    /// �����ε� :  �������Ĺ��
    /// </summary>
    /// <param name="listData">����Ʈ ���</param>
    /// <param name="length">����Ʈ ũ��</param>
    private static void SelectionSort(List<T> listData, int length, SortType type)
    {
        int i = 0;
        int j = 0;
        int min = 0;
        for (i = 0; i < length - 1; i++) // �迭ũ�⸸ŭ ���������� ������
        {
            min = i; //�ּҰ��� ã������ ��ġ���� �����Ѵ�
            for (j = i + 1; j < length; j++) //-������ �ּҰ������̳������̴� +������ Ž���� �����Ѵ�.
            {
                if (SortAscDesCheck(listData[j], listData[min] ,type))// min���� ���ؼ� min���� �������� �ִ°�� 
                {
                    min = j;//�ּҰ���ġ�� �����Ѵ�.
                }//�ݺ��Ͽ� ������ü�� �ּҰ���ġ�� ã�´�.
            }
            if (i != min)
            {
                //�ּҰ��� ���� ���� ������������ ��ü�Ѵ�.
                //�Լ��ξȻ������� �Լ��λ��¼��� T[i] ���� �Ű������� �ѱ涧 ����Ÿ���� �ƴ϶� ��Ÿ������ ġȯ�� �Ǽ� �Ѿ�⶧���̴�.�����¸𸣰ٴ�.
                T tempObj = listData[i];
                listData[i] = listData[min];
                listData[min] = tempObj;
            }
        }
    }

    
    /// <summary>
    /// �������� ������ �ΰ�ü�� ���Ѵ�.
    /// </summary>
    /// <param name="arrayData">�迭 ���</param>
    /// <param name="length">�迭 ũ��</param>
    private static void BubbleSort(T[] arrayData, int length, SortType type)
    {
        int i = 0;
        int j = 0;

        for (i = 0; i < length - 1; i++) //ù��°���� ���������ͱ��� �� 
        {
            for (j = 0; j < length - 1 - i; j++) // �ڷΰ����� ���� ��Ƚ���� �ٿ�����.
            {
                if (SortAscDesCheck(arrayData[j + 1], arrayData[j],type))//�ڿ� ���� ���簪�� ���ؼ� �ڿ����� ������ 
                {
                    //�Լ��ξȻ������� �Լ��λ��¼��� T[i] ���� �Ű������� �ѱ涧 ����Ÿ���� �ƴ϶� ��Ÿ������ ġȯ�� �Ǽ� �Ѿ�⶧���̴�. ref ���� ��������� ��=> 
                    //���簪�� �ڿ����� ��ü�Ѵ�. 
                    T tempObj = arrayData[j];
                    arrayData[j] = arrayData[j + 1];
                    arrayData[j + 1] = tempObj;
                }
            }
        }
    }

    /// <summary>
    /// �����ε� : ���� ���� 
    /// </summary>
    /// <param name="listData">����Ʈ ���</param>
    /// <param name="length">����Ʈ�� ũ��</param>
    private static void BubbleSort(List<T> listData, int length , SortType type)
    {
        int i = 0;
        int j = 0;

        for (i = 0; i < length - 1; i++) //ù��°���� ���������ͱ��� �� 
        {
            for (j = 0; j < length - 1 - i; j++) // �ڷΰ����� ���� ��Ƚ���� �ٿ�����.
            {
                if (SortAscDesCheck(listData[j + 1], listData[j], type))//�ڿ� ���� ���簪�� ���ؼ� �ڿ����� ������ 
                {
                    //�Լ��ξȻ������� �Լ��λ��¼��� T[i] ���� �Ű������� �ѱ涧 ����Ÿ���� �ƴ϶� ��Ÿ������ ġȯ�� �Ǽ� �Ѿ�⶧���̴�. ref ���� ��������� ��=> 
                    //���簪�� �ڿ����� ��ü�Ѵ�. 
                    T tempObj = listData[j];
                    listData[j] = listData[j + 1];
                    listData[j + 1] = tempObj;
                }
            }
        }
    }








}
/*


�Ʒ������� �ð����� �߰��ҿ��� 

4. ���� ����(Merge Sort)

���� : �� �̻��� �κ��������� ������, �� �κ������� ������ ���� �κ����յ��� �ٽ� ���ĵ� ���·� ��ġ�� ���. �������� �����Ѵ�.

- ���� ������ ���(Divide-And-Conquer)

���� : �ذ��ϰ��� �ϴ� ������ ���� ũ���� ������ ������� �����Ѵ�.
���� : ������ ���� ������ ��ȯ������ �ذ��Ѵ�.
�պ� : ���� ������ �ظ� ���Ͽ�(merge) ���� ������ ���� �ظ� ���Ѵ�.







#include<iostream>
using namespace std;
#define ARRNUM 5
int N = ARRNUM;
int arr[] = { 8,5,3,1,6 };
int tempArr[ARRNUM];

void Merge(int left, int right)
{
    //����¥�� arr�� tempArr�������Ѵ�.
    for (int i = left; i <= right; i++)
    {
        tempArr[i] = arr[i];
    }

    int mid = (left + right) / 2;

    int tempLeft = left;
    int tempRight = mid+1;
    int curIndex = left;

    //temparr�迭 ��Ⱥ��. ���� ���ݰ� ������ ���� ���ؼ�
    //�� ���� ���� ���� �迭�� ����
    while (tempLeft <= mid && tempRight <= right)
    {
        if (tempArr[tempLeft] <= tempArr[tempRight])
        {
            arr[curIndex++] = tempArr[tempLeft++];			
        }
        else
        {
            arr[curIndex++] = tempArr[tempRight++];			
        }		
    }
    //���� ���ݿ� ���� ���ҵ��� ���� �迭�� ����
    int remain = mid - tempLeft;
    for (int i = 0; i <= remain; i++)
    {
        arr[curIndex + i] = tempArr[tempLeft + i];
    }
}
void Partition( int left, int right)
{
    if (left < right)
    {
        int mid = (left + right) / 2;
        Partition(left, mid);
        Partition(mid + 1, right);
        Merge(left, right);
    }
}
int main() {



    Partition(0, N - 1);

    for (int i = 0; i < N; i++)
    {
        cout << arr[i] << endl;
    }

    return 0;
}





5. �� ����

���� : Ʈ�� ������� �ִ� �� Ʈ��or �ּ� �� Ʈ���� ������ ������ �ϴ� ���. ������ ���� X

�������� ������ ���ؼ��� �ִ� ���� �����ϰ� �������� ������ ���ؼ��� �ּ� ���� �����ϸ� �ȴ�. 





�ڵ����





6. �� ����(Quick Sort)(��������)

��� :  ������ ���ճ��� ������ ����(pivot)���� ���ϰ� �ش� �ǹ����� ������ �������� �ΰ��� �κ� �������� ������.

  ���� �κп��� �ǹ������� �������鸸, �ٸ� ������ ū���鸸 �ִ´�. ������ ���� X

�� �̻� �ɰ� �κ� ������ ���� ������ ������ �κ� ���տ� ���� �ǹ�/�ɰ��� ��������� ����.






#include<iostream>
using namespace std;
#define ARRNUM 8
int N = ARRNUM;
int arr[] = { 8,15,5,9,3,12,1,6};
void Swap(int& A, int& B)
{
    int Temp = A;
    A = B;
    B = Temp;
}


int Partition( int left, int right)
{
    int pivot = arr[right]; //�� �������� �Ǻ� ����
    int i = (left - 1);

    for (int j = left; j <= right-1; j++)
    {
        if (arr[j] <= pivot) //�迭 ��ȸ�ϸ� �Ǻ��̶� ���ų� ���� �� Ž��
        {					
            i++;    //i �ε��� ��ġ�� ��ü 
            Swap(arr[i], arr[j]);
        }
    }

    //�� ã�� �ǿ����ʿ� �ִ� �Ǻ����� ��ü
    Swap(arr[i + 1], arr[right]);

    return (i + 1); // ���ϰ� �������� ������ �����ε������� �۰� �������� ū����

}

void Quick(int L, int R)
{
    if (L < R)
    {
        int p = Partition(L, R); //�ѹ� �Ǻ����� ������ �� �������� 

        Quick(L, p - 1); //�Ǻ� ���� ���� �ٽ� ����
        Quick(p + 1, R); //�Ǻ� ���� ������ �ٽ� ����
    }
}
int main() {

    Quick(0, N - 1);
    for (int i = 0; i < N; i++)
    {
        cout << arr[i] << endl;
    }

    return 0;
}

6-2





#include<iostream>
using namespace std;
#define ARRNUM 8
int N = ARRNUM;
int arr[] = { 2,15,5,9,3,12,20,6 };
void Swap(int& A, int& B)
{
    int Temp = A;
    A = B;
    B = Temp;
}

void QuickSort(int left, int right)
{
    int pivot = arr[(left+right)/2]; //�Ǻ� �߽� ����
    int startIndex = left; 
    int endIndex = right;

    while (startIndex <= endIndex) //startIndex�� endIndex���� ������������ while
    {
        while (arr[startIndex] < pivot) //�ǹ����� ���ʿ��� �ǹ����� ū�� ã��
        {
            ++startIndex;
        }
        while (arr[endIndex] > pivot) //�ǹ����� �����ʿ��� �ǹ����� ������ ã��
        {
            --endIndex;
        }

        if (startIndex <= endIndex) //�׷��� ã���� ���� ������ ���� ���� swap
        {
            Swap(arr[startIndex], arr[endIndex]);
            ++startIndex;
            --endIndex;
        }
    }

    if (left < endIndex) //�ǹ����� ���� smaller�� ����
    {
        QuickSort(left, endIndex);
    } 
    if (startIndex < right)//�ǹ����� ������ bigger�� ����
    {
        QuickSort(startIndex, right);
    }
}


int main() {

    QuickSort(0, N - 1);
    for (int i = 0; i < N; i++)
    {
        cout << arr[i] << endl;
    }

    return 0;
}




7. ��� ����(Radix Sort)

��� : ���� �ڸ������� ���ذ��� �����Ѵ�. �񱳿����� ���� �ʾ� ��������, �� �ٸ� �޸� ������ �ʿ��ϴٴ°� ����.��������� ���� �ڸ������� ���Ͽ� ������ ���ٴ� ���� �⺻ �������� �ϴ� ���� �˰����Դϴ�. ��������� �� ������ ���� ������ ���� �ӵ��� �������� ������ ��ü ũ�⿡ ��� ���̺��� ũ�⸸�� �޸𸮰� �� �ʿ��մϴ�.


void Radix_Sort()
{
    int Radix = 1;
    while(true){
        if(Radix >= n)
        {
            break;
        }
        Radix = Radix *10;
    }

    for(int i = 1; i < Radix; i = i *10)
    {
        for(int j = 0; j < n; j++)
        {
            int k;
            if (arr[j] < i)
            {
                k = 0;
            }
            else
            {
                k = (arr[j] / i) % 10;
                Q[k].push(arr[j]);
                Q[j].pop();
                Idx++;
            }
        }
    }
}

- ��� ǥ��(�ð����⵵)


d�� �ڸ���



 */

