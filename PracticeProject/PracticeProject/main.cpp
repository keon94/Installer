#include <iostream>
#include <functional>
#include <string>
#include "vld.h"
#include <ctime>

inline int fact(int x){ if (x == 0) return 1; else return x*fact(x - 1); }
template <typename T> T* _copy(const T* a, int size){
	T* b = new T[size];
	memcpy(b, a, size*sizeof(T));
	return b;
}

template <typename T> void _copy(const T* src, T* dest, int size){
	if (size < 1) throw;
	else if (!src || !dest) throw;
	for (int i = 0; i < size; ++i)
		dest[i] = src[i];
}

template <typename T> void print(T* a, int size, std::string name = ""){
	if (name != ""){
		std::cout << "1Darray " << name << ":  ";
	}
	for (int i = 0; i < size; ++i)
		std::cout << a[i] << " ";
	std::cout << std::endl;
}

template <typename T> void print(T** a, int x_size, int y_size, std::string name = ""){
	if (name != ""){
		std::cout << "2Darray " << name << ":\n";
	}
	for (int i = 0; i < x_size; ++i)
		print(a[i], y_size, name + "[" + std::to_string(i) + "]");
	std::cout << std::endl;
}


namespace{
	template <typename T> T** _2DArray(int x, int y){
		T** a = new T*[x]();
		for (int i = 0; i < x; ++i){
			a[i] = new T[y]();
		}
		return a;
	}
	template <typename T> inline void _del_2DArray(T** a, int x){
		for (int i = 0; i < x; ++i) delete[] a[i];
		delete[] a;
	}

	template <typename T> inline void leftCyclicShift(T* a, int size){
		if (size < 1) throw;
		T a0 = *a;
		for (int i = 1; i < size; ++i) a[i - 1] = a[i];
		a[size - 1] = a0;
	}
	
	template <typename T> T** perm(T* arr, int size){
		T** y = _2DArray<T>(fact(size), size);
		T** y0 = y;
		std::function<void(T*,T*,int)> Perm;
		clock_t t0, tf;
		t0 = clock();
		Perm = [&](T* local_arr, T* right_ptr, int rightSize)
		{			
			if (rightSize == 1){
				//_copy(local_arr, *y, size);
				memcpy(*y, local_arr, size*sizeof(T));
				y++;
				return;
			}
			unsigned int offset = right_ptr - local_arr;
			T* local_arr_copy = _copy(local_arr, size);
			T* right_ptr_copy = local_arr_copy; 
			right_ptr_copy += offset;

			for (int i = 0; i < rightSize; ++i){
				Perm(local_arr_copy, right_ptr_copy + 1, rightSize - 1);
				leftCyclicShift(local_arr_copy + offset, size - offset);
			}
			delete[] local_arr_copy;

		};
		Perm(arr, arr, size);
		tf = clock();
		printf("\nTime: %f seconds\n", ((float)(tf-t0)) / CLOCKS_PER_SEC);
		return y0;
	}
	

	template <typename T> void cyclic_copy(T* src, T* start_pt, T* dest, int size)
	{
		T* last = src + size - 1;
		if (start_pt == last || !src || !dest) throw;
		if (size == 1){
			dest = src;
			return;
		}
		int i, first_index = start_pt - src;
		T* iter = nullptr;
		for (i = 0; i < first_index; ++i)
			dest[i] = src[i];
		for (iter = start_pt + 1;; ++i){
			dest[i] = *iter;
			if (iter == start_pt) return;
			else if (iter == src + size - 1) iter = start_pt;			
			else iter++;
		}
	}
	


}

template <typename T> T* testArray(int size){
	T* x = new T[size]();
	for (int i = 0; i < size; ++i)
		x[i] = static_cast<T>(i);
	return x;
}

#define MAX_RAND 20

template <typename T> T* randomArray(int size){
	T* x = new T[size]();
	for (int i = 0; i < size; ++i)
		x[i] = static_cast<T>(rand() % MAX_RAND + 1);
	return x;
}

int main(){
	srand((unsigned)time(NULL));
	int size = 4;
	int* x = testArray<int>(size);
	print(x, size);
	//int* z = new int[size]();
	//cyclic_copy(x, x + 3, z, size);
	//print(z, size);
	//int* x = randomArray<int>(size);
	int** y = perm(x, size);
	delete[] x;
	//delete[] z;
	print(y, fact(size), size);
	_del_2DArray(y, fact(size));
	printf("done\n");
}





