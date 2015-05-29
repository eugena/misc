using System;
using System.Data;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.IO;
using System.Text;
using System.Diagnostics;

/**
 * Предлагаемая структура данных для поиска представляет собой
 * модифицированное под задачу trie:
 * 1. Все строки словаря состоят из 3 символов (a,b,c), поэтому нам нужно всего 3 указателя для каждого узла, 
 * 		в общем случае (для латинского словаря нужно было бы 26) ситуация бы усложнилась.
 * 2. Каждый узел содержит список строк, которые начинаются с подстроки, образованной символами от текущего узла до корня,
 *      что позволяет нам избавиться от рекурсии при сборке списков. Когда подстрока найдена, мы просто берем 
 *      готовый список из текущего узла.
 *      Более того, нам не интересны, слова с "маленькими" весами и мы можем их не хранить в узлах, 
 *      другими словами, список содержит всего 10 слов с наибольшими весами. 
 * 3. При добавлении слова в дерево, мы всегда имеем отсортированнный как нам надо спискок слов в каждом узле из-за того, 
 *      что слова добавляются в "правильном" порядке, с сортировкой (вес desc, слово).
 * 
 * Время разное:
 *    MonoDeveloperConcole: 1390 ms
 *    Linux terminal: 3153 ms
 *    
 **/ 
namespace textEditorKontur
{
	class MainClass
	{
		public static void Main (string[] args) {
			var timer = new Stopwatch();
			timer.Start();

			StreamReader sr= new StreamReader(Console.OpenStandardInput());
			string line, aline;
			uint i = 0, n= 0, m = 0;
			jTrie trie = new jTrie();
			Dictionary<string, uint> Data = new Dictionary<string, uint>();
			do {
				line = sr.ReadLine();
				if (line != null) {
					if (i == 0) { // в начале файла
						n = Convert.ToUInt32(line);
					} else {
						if (i == n + 1) { // дочитали весь словарь - строим дерево
							var words =
								from d in Data
								orderby d.Value descending, d.Key 
								select d.Key;
							foreach (string w in words) {
								trie.Add(w);
							}
							m = Convert.ToUInt32 (line);
						} else if (m == 0) { // считываем словарь
							string[] d = line.Split (' ');
							Data.Add (d[0], Convert.ToUInt16(d[1]));
						} else {  // ищем подстроку
							Queue<string> found = trie.Find (line);
							if (found.Count() > 0) {
								Console.WriteLine (string.Join ("\n", found));
							}
						}
					}
				}
				i++;
			} while (line != null);

			timer.Stop();
			Console.WriteLine("elapsed time = {0} ms", timer.ElapsedMilliseconds);
		}

		public class jTrie
		{
			private jTrieNode t_root = null;
			const short qty = 10;

			private void Add(string s, int pos, ref jTrieNode node)	{
				if (node == null) {
					node = new jTrieNode(s[pos], 0);
				}
				if (s[pos] == node.n_char) {
					if (node.n_words.filled == 0) { 
						int count = node.n_words.Count();
						if (count < qty)	{
							node.n_words.Enqueue(s);
						}
						if (count == qty - 1)	{
							node.n_words.filled = 1;
						}
					}
					if (pos + 1 == s.Length) { 
						node.n_end = 1;
					} else {
						Add(s, pos + 1, ref node.n_center); 
					}
				} else if (s[pos] < node.n_char) {
					Add(s, pos, ref node.n_left);
				} else if (s[pos] > node.n_char) {
					Add(s, pos, ref node.n_right); 
				}
			}

			public void Add(string s) {
				Add(s, 0, ref t_root);
			}

			public Queue<string> Find(string s)	{
				int pos = 0;
				jTrieNode node = t_root;
				while (node != null) {
					if (s[pos] == node.n_char) {
						if (++pos == s.Length) {
							return node.n_words; 
						}
						node = node.n_center;
					} else if (s[pos] < node.n_char) { 
						node = node.n_left; 
					} else if (s[pos] > node.n_char) { 
						node = node.n_right; 
					}
				}
				return new Queue<string>();
			}

			class jTrieNode
			{
				internal char n_char;
				internal jTrieNode n_left, n_center, n_right;
				internal byte n_end;
				internal jQueue n_words = new jQueue();

				public jTrieNode(char ch, byte end)	{
					n_char = ch;
					n_end = end;
				}
			}
			
			class jQueue : Queue<string>
			{
				internal byte filled = 0;
			}
		}
	}
}
