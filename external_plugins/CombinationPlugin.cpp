#include "CombinationPlugin.h"

enum Suits
{
	S_SPADES,
	S_HEARTS,
	S_DIAMONDS,
	S_CLUBS
};

struct Card
{
	Card() :suit(0), value(0)
	{
	}

	Card(uint8_t suit, uint8_t value)
		:suit(suit),
		value(value)
	{
	}

	uint8_t suit;
	uint8_t value;
};

const bool operator < (const Card& card1, const Card& card2) { return card1.value > card2.value; }
const bool operator == (const Card& card1, const Card& card2) { return card1.value == card2.value; }

enum CombinationTypes
{
	C_NO_COMBINATION,
	C_HIGH_CARD,
	C_PAIR,
	C_TWO_PAIRS,
	C_THREE_OF_A_KIND,
	C_STRAIGHT,
	C_FLUSH,
	C_FULL_HOUSE,
	C_FOUR_OF_A_KIND,
	C_STRAIGHT_FLUSH,
	C_ROYAL_FLUSH
};

struct Combination
{
	Combination()
		: cards(5) // like a Card cards[5];
	{

	}

	uint8_t type = C_NO_COMBINATION;
	std::vector<Card> cards;
};

int getCombination(const std::multiset<Card>& cards, Combination& combo);

const bool operator > (const Combination& combo1, const Combination& combo2);

void setSuitsByGraterValue(std::set<Card>& spades,
	std::set<Card>& hearts,
	std::set<Card>& diamonds,
	std::set<Card>& clubs,
	const std::multiset<Card>& cards)
{
	for (auto it = cards.begin(); it != cards.end(); it++)
	{
		if ((*it).suit == S_SPADES)
			spades.insert(*it);
		else if ((*it).suit == S_HEARTS)
			hearts.insert(*it);
		else if ((*it).suit == S_DIAMONDS)
			diamonds.insert(*it);
		else if ((*it).suit == S_CLUBS)
			clubs.insert(*it);
	}
}

bool getBestFlushInSuit(const std::set<Card>& cards, Combination& combo)
{
	if (cards.size() < COMBO_SIZE)
		return false;

	uint8_t index_in_combo = 0;

	for (auto it_cur = cards.begin(); it_cur != cards.end(); it_cur++)
	{
		combo.cards[index_in_combo] = *it_cur;

		auto it_next = std::next(it_cur, 1);
		if (it_next == cards.end())
			break;

		if (it_cur->value - 1 == it_next->value)
		{
			if (++index_in_combo == MAX_INDEX_IN_COMBO)
			{
				combo.cards[index_in_combo] = *it_next;
				combo.type = (combo.cards[0].value == ACE_CARD) ? C_ROYAL_FLUSH : C_STRAIGHT_FLUSH;
				return true;
			}
		}
		else
			index_in_combo = 0;
	}

	// check a wheel = 5 4 3 2 + A
	if ((combo.cards[0].value == 5) && (index_in_combo == 3))
	{
		if ((*cards.begin()).value == ACE_CARD)
		{
			combo.cards[MAX_INDEX_IN_COMBO] = *cards.begin();
			combo.type = C_STRAIGHT_FLUSH;
			return true;
		}
	}

	// just simple flush
	std::copy_n(cards.begin(), COMBO_SIZE, combo.cards.begin());
	combo.type = C_FLUSH;
	return true;
}

Combination getBestFlushCombo(const Combination& combo1, const Combination& combo2)
{
	if (combo1.type > combo2.type)
		return combo1;

	if (combo2.type > combo1.type)
		return combo2;

	if (std::lexicographical_compare(combo1.cards.begin(), combo1.cards.end(),
		combo2.cards.begin(), combo2.cards.end()))
		return combo1;
	else return combo2;
}

int getBestFlush(const std::multiset<Card>& cards, Combination& combo)
{
	if (cards.size()<5)
		return 0;

	combo.type = C_NO_COMBINATION;

	std::vector< std::set<Card> >   suits(4); // spades hearts diamonds clubs
	std::vector< Combination >      tmp_combo(4);

	setSuitsByGraterValue(suits[0], suits[1], suits[2], suits[3], cards);

	for (int i = 0; i< 4; i++)
	{
		if (getBestFlushInSuit(suits[i], tmp_combo[i]) == false)
			continue;

		combo = tmp_combo[i];

		if (tmp_combo[i].type == C_ROYAL_FLUSH)
			return combo.type;

		if (i > 0) // compare with previous
			combo = getBestFlushCombo(tmp_combo[i - 1], tmp_combo[i]);
	}

	return combo.type;
}

int getBestFourOfAKind(const std::multiset<Card>& cards, Combination& combo)
{
	if (cards.size() < 5)
		return 0;

	uint8_t index_in_combo = 0; // if this index == 3 then we have four of a kind!

	for (auto it_cur = cards.begin(); it_cur != cards.end(); it_cur++)
	{
		combo.cards[index_in_combo] = *it_cur;

		auto it_next = std::next(it_cur, 1);
		if (it_next == cards.end())
			break;

		if ((*it_cur).value == (*it_next).value)
		{
			if (++index_in_combo == 3)
			{
				combo.cards[index_in_combo] = *it_next;

				// we always have previous or next card in this case
				if ((*cards.begin()).value != (*it_cur).value)
					combo.cards[MAX_INDEX_IN_COMBO] = *cards.begin();
				else
					combo.cards[MAX_INDEX_IN_COMBO] = *(++it_next);

				combo.type = C_FOUR_OF_A_KIND;
				return combo.type;
			}
		}
		else
			index_in_combo = 0;
	}

	return 0;
}

Combination getBestFourOfAKindCombo(const Combination& combo1, const Combination& combo2)
{
	if (combo1.cards[0].value > combo2.cards[0].value)
		return combo1;
	else
		return combo2;
}

int getThreeOfAKindOnly(const std::multiset<Card>& cards, Combination& combo)
{
	if (cards.size()<3)
		return 0;

	uint8_t cards_in_line = 1;

	for (auto it_cur = cards.begin(); it_cur != cards.end(); it_cur++)
	{
		auto it_next = std::next(it_cur, 1);
		if (it_next == cards.end())
			break;

		if ((*it_cur).value == (*it_next).value)
			cards_in_line++;
		else
			cards_in_line = 1;

		if (cards_in_line == 3) // have three of a kind
		{
			it_cur--;

			for (int i = 0; i<3; i++)
			{
				combo.cards[i] = *it_cur;
				it_cur++;
			}
			combo.type = C_THREE_OF_A_KIND;
			return combo.type;
		}
	}
	return 0;
}

int getBestFullHouse(const std::multiset<Card>& cards, Combination& combo)
{
	if (cards.size()<5)
		return 0;

	std::multiset<Card> remaining_cards(cards);

	// looking for three of a kind
	if (!getThreeOfAKindOnly(cards, combo))
		return 0;

	for (int i = 0; i<3; i++)
	{
		auto it_for_remove = remaining_cards.find(combo.cards[i]);
		remaining_cards.erase(it_for_remove);
	}

	// now looking for a pair in remaining cards
	for (auto it_cur = remaining_cards.begin(); it_cur != remaining_cards.end(); it_cur++)
	{
		auto it_next = std::next(it_cur, 1);
		if (it_next == remaining_cards.end())
			break;

		if ((*it_cur).value == (*it_next).value) // got it!
		{
			combo.cards[3] = *it_cur;
			combo.cards[MAX_INDEX_IN_COMBO] = *it_next;
			combo.type = C_FULL_HOUSE;
			return combo.type;
		}
	}

	return 0;
}

int getBestStraight(const std::multiset<Card>& cards, Combination& combo)
{
	if (cards.size()<5)
		return 0;

	uint8_t cards_in_line = 1;

	for (auto it_cur = cards.begin(); it_cur != cards.end(); it_cur++)
	{
		combo.cards[cards_in_line - 1] = *it_cur;

		auto it_next = std::next(it_cur, 1);
		if (it_next == cards.end())
			break;

		if ((*it_cur).value == (*it_next).value)
			continue;

		if ((*it_cur).value - 1 == (*it_next).value)
			cards_in_line++;
		else
			cards_in_line = 1;

		if (cards_in_line == 5) // got it!
		{
			combo.cards[MAX_INDEX_IN_COMBO] = *it_next;
			combo.type = C_STRAIGHT;
			return combo.type;
		}
	}

	return 0;
}

int getBestThreeOfAKind(const std::multiset<Card>& cards, Combination& combo)
{
	if (cards.size()<5)
		return 0;

	std::multiset<Card> remaining_cards(cards);

	if (!getThreeOfAKindOnly(cards, combo))
		return 0;

	for (int i = 0; i<3; i++)
	{
		auto it_for_remove = remaining_cards.find(combo.cards[i]);
		remaining_cards.erase(it_for_remove);
	}

	auto it = remaining_cards.begin();
	combo.cards[3] = *it;
	combo.cards[MAX_INDEX_IN_COMBO] = *(++it);
	combo.type = C_THREE_OF_A_KIND;
	return combo.type;
}

int getBestPairOnly(const std::multiset<Card>& cards, Combination& combo)
{
	for (auto it_cur = cards.begin(); it_cur != cards.end(); it_cur++)
	{
		auto it_next = std::next(it_cur, 1);
		if (it_next == cards.end())
			break;

		if ((*it_cur).value == (*it_next).value) // got first pair!
		{
			combo.cards[0] = *it_cur;
			combo.cards[1] = *it_next;
			return C_PAIR;
		}
	}

	return 0;
}

int getBestTwoPairs(const std::multiset<Card>& cards, Combination& combo)
{
	if (cards.size()<5)
		return 0;

	std::multiset<Card> remaining_cards(cards);
	Combination tmp_combo;
	uint8_t combo_index = 0;
	uint8_t pair_conts = 2;

	while (pair_conts--)
	{
		if (getBestPairOnly(remaining_cards, tmp_combo) == 0)
		{
			return 0;
		}

		for (int i = 0; i<2; i++)
		{
			combo.cards[combo_index] = tmp_combo.cards[i];
			auto it_for_remove = remaining_cards.find(combo.cards[combo_index++]);
			remaining_cards.erase(it_for_remove);
		}
	}

	combo.cards[MAX_INDEX_IN_COMBO] = *remaining_cards.begin();
	combo.type = C_TWO_PAIRS;
	return C_TWO_PAIRS;
}

int getBestPair(const std::multiset<Card>& cards, Combination& combo)
{
	if (cards.size()<5)
		return 0;

	std::multiset<Card> remaining_cards(cards);
	uint8_t combo_index = 0;

	if (getBestPairOnly(remaining_cards, combo) == 0)
		return 0;

	for (int i = 0; i<2; i++)
	{
		auto it_for_remove = remaining_cards.find(combo.cards[i]);
		remaining_cards.erase(it_for_remove);
	}

	auto it = remaining_cards.begin();
	for (int index = 2; index <= MAX_INDEX_IN_COMBO; index++)
	{
		combo.cards[index] = *(it++);
	}

	combo.type = C_PAIR;
	return C_PAIR;
}

int getTheHighestCard(const std::multiset<Card>& cards, Combination& combo)
{
	if (cards.size() < 5)
		return 0;

	auto it = cards.begin();
	for (int i = 0; i <= MAX_INDEX_IN_COMBO; i++)
		combo.cards[i] = *(it++);

	combo.type = C_HIGH_CARD;
	return combo.type;
}

// return type of combo
int getCombination(const std::multiset<Card>& cards, Combination& combo)
{
	if (cards.size()<5)
		return 0;

	bool have_flush = false;
	Combination flush_combo;

	int res = getBestFlush(cards, combo);
	if (res == C_ROYAL_FLUSH || res == C_STRAIGHT_FLUSH)
	{
		return combo.type;
	}

	if (res == C_FLUSH)
	{
		flush_combo = combo;
		have_flush = true;
	}

	res = getBestFourOfAKind(cards, combo);
	if (res == C_FOUR_OF_A_KIND)
	{
		return combo.type;
	}

	res = getBestFullHouse(cards, combo);
	if (res == C_FULL_HOUSE)
	{
		return combo.type;
	}

	if (have_flush)
	{
		combo = flush_combo;
		return combo.type;
	}

	res = getBestStraight(cards, combo);
	if (res == C_STRAIGHT)
	{
		return combo.type;
	}

	res = getBestThreeOfAKind(cards, combo);
	if (res == C_THREE_OF_A_KIND)
	{
		return combo.type;
	}

	res = getBestTwoPairs(cards, combo);
	if (res == C_TWO_PAIRS)
	{
		return combo.type;
	}

	res = getBestPair(cards, combo);
	if (res == C_PAIR)
	{
		return combo.type;
	}

	res = getTheHighestCard(cards, combo);
	if (res == C_HIGH_CARD)
	{
		return combo.type;
	}

	return 0;
}

const bool operator > (const Combination& combo1, const Combination& combo2)
{
	if (combo1.type > combo2.type)
		return true;

	if (combo1.type < combo2.type)
		return false;

	switch (combo1.type)
	{
	case C_HIGH_CARD:
	case C_PAIR:
	case C_TWO_PAIRS:
	case C_FLUSH:
	{
		for (int i = 0; i<combo1.cards.size(); i++)
		{
			if (combo1.cards[i] == combo2.cards[i])
				continue;

			return combo1.cards[i].value > combo2.cards[i].value;
		}
		return false;
	}
	case C_THREE_OF_A_KIND:
	case C_STRAIGHT:
	case C_FULL_HOUSE:
	case C_FOUR_OF_A_KIND:
	case C_STRAIGHT_FLUSH:
	{
		return combo1.cards[0].value > combo2.cards[0].value;
	}

	case C_ROYAL_FLUSH:
		return false;

	default:
		return true; // C_NO_COMBO
	}
	return false;
}


// ========================================================================================== //



// ========================================================================================== //

/**
@brief Сущность данных которыми оперирует алгоритм ГОСТ 28147-89,
для удобства разбит uniun'ом на подтипы.

@param m_lData Тип ULONG64 1 X 64bit.
@param m_chData Тип UINT8 8 X 8Bit.
@param m_iData Тип UINT32 2 X 32Bit.
*/
struct GostData
{
	union
	{
		ULONG64 m_lData;
		UINT8 m_chData[8];
		UINT32 m_iData[2];
	};
};

/**
@brief Массив смещений, требуется для основного прохода алгорима ГОСТ 28147-89
*/
static const int g_iKeyOffset[32] =
{
	0, 1, 2, 3, 4, 5, 6, 7,
	0, 1, 2, 3, 4, 5, 6, 7,
	0, 1, 2, 3, 4, 5, 6, 7,
	7, 6, 5, 4, 3, 2, 1, 0
};

// ========================================================================================== //
inline unsigned int GetBit(void * src, unsigned int pos)
{
	unsigned char* ptr = (unsigned char*)src + pos / CHAR_BIT;
	return (*ptr >> (pos % CHAR_BIT)) & 1;
}
// ========================================================================================== //
inline void SetBit(void* dst, unsigned int pos, unsigned int value)
{
	unsigned char* ptr = (unsigned char*)dst + pos / CHAR_BIT;
	unsigned bit_num = pos % CHAR_BIT;
	*ptr = (*ptr &~(1 << bit_num)) | ((value & 1) << bit_num);
}
// ========================================================================================== //
unsigned int GetRandom(const unsigned int tFrom, const unsigned int tTo)
{
	std::random_device rd;
	std::seed_seq seed{ rd(), rd(), rd(), rd(), rd(), rd(), rd(), rd() };
	std::mt19937 g(seed);
	std::uniform_int_distribution<unsigned int> uni(tFrom, tTo);
	return uni(g);
}
// ========================================================================================== //
void DoGenerateKey(struct CGost89Crypt* inData)
{
	memset(inData->m_uiKey, -1, 8 * sizeof(unsigned int));

	unsigned int i;
	for (i = 0; i < 8; i++)
		inData->m_uiKey[i] = GetRandom(0, RAND_MAX);
}
// ========================================================================================== //
void DoGenerateTable(struct CGost89Crypt* inData)
{
	memset(inData->m_iTable, -1, 128);

	int iCompleted;
	BOOL bBadValue;

	unsigned int i;
	unsigned int z;
	for (i = 0; i < 8; i++)
	{
		iCompleted = 0;
		while (iCompleted < 16)
		{
			int iValue = GetRandom(0, 15);
			bBadValue = false;

			for (z = 0; z < 16; z++)
			{
				if (inData->m_iTable[i][z] == iValue)
					bBadValue = true;
			}

			if (!bBadValue)
			{
				inData->m_iTable[i][iCompleted] = iValue;
				iCompleted++;
			}
		}
	}
}
// ========================================================================================== //
ULONG64 GetRGPCH(ULONG64 inData)
{
	UINT32 N1;
	UINT32 N2;

	N1 = (UINT32)inData;
	N2 = inData >> 32;

	N1 = (N1 + C1) % 0x10000000;
	N2 = (N2 + C2) % 0xFFFFFFFF;

	inData = N2;
	inData = (inData << 32) | N1;
	return inData;
}
// ========================================================================================== //
void DoMainStep(struct GostData* inData, BOOL bMac, BOOL bCrypt, const struct CGost89Crypt* inCryptEntity)
{
	unsigned int iLeft = inData->m_iData[0]; // Разбиваем на 2 блока.
	unsigned int iRight = inData->m_iData[1];
	unsigned int iSum = 0;

	unsigned int iMaxLoop;
	if (bMac) // Выработка имитовставки.
	{
		iMaxLoop = 16;
		bCrypt = true;
	}
	else
		iMaxLoop = 32;

	unsigned int i;
	unsigned int z;
	for (z = 0; z < iMaxLoop; z++)
	{
		if (bCrypt)    //  S = N1 + X (mod 2 в 32).
			iSum = iLeft + inCryptEntity->m_uiKey[g_iKeyOffset[z]] % INT_MAX;

		else
			iSum = iLeft + inCryptEntity->m_uiKey[g_iKeyOffset[31 - z]] % INT_MAX;

		int iBitArray[8];
		memset(iBitArray, 0, sizeof(int) * 8);

		// Число S разбивается на 8 частей: S0,S1,S2,S3, S4,S5,S6,S7 по 4 бита каждая, где S0 - младшая, а S7 - старшая части числа S.
		int iOffset = 31;
		for (i = 0; i < 8; i++)
		{
			SetBit(&iBitArray[i], 0, GetBit(&iSum, iOffset));
			iOffset--;
			SetBit(&iBitArray[i], 1, GetBit(&iSum, iOffset));
			iOffset--;
			SetBit(&iBitArray[i], 2, GetBit(&iSum, iOffset));
			iOffset--;
			SetBit(&iBitArray[i], 3, GetBit(&iSum, iOffset));
			iOffset--;
		};


		// Для всех i от 0 до 7: Si = T(i, Si), где T(a, b) означает ячейку таблицы замен с номером строки a и номером столбца b (счет с нуля).
		for (i = 0; i < 8; i++)
			iBitArray[i] = inCryptEntity->m_iTable[i][iBitArray[i]];

		iOffset = 0; // Укладываем обратно.
		for (i = 0; i < 8; i++)
		{
			SetBit(&iSum, iOffset, GetBit(&iBitArray[i], 0));
			iOffset++;

			SetBit(&iSum, iOffset, GetBit(&iBitArray[i], 1));
			iOffset++;

			SetBit(&iSum, iOffset, GetBit(&iBitArray[i], 2));
			iOffset++;

			SetBit(&iSum, iOffset, GetBit(&iBitArray[i], 3));
			iOffset++;
		};

		// Новое число S, полученное на предыдущем шаге циклически сдвигается в сторону старших разрядов на 11 бит.
		iSum = (iSum << 11) | (iSum >> 21);

		// S = S xor N2, где xor - операция исключающего или.
		iSum = iRight ^ iSum;

		iRight = iLeft; //# N2 = N1.
		iLeft = iSum;  //# N1 = S.
	}
	inData->m_iData[0] = iRight;
	inData->m_iData[1] = iLeft;
}
// ========================================================================================== //
void GostGammaBlockEncode(struct GostData* inData, unsigned int iBlockCount, ULONG64 inKey, const struct CGost89Crypt* inCryptEntity)
{
	unsigned int i = 0;
	for (i = 0; i < iBlockCount; i++)
	{
		inKey = GetRGPCH(inKey); // Инициализируем РГПСЧ зашифрованной синхропосылкой.
		DoMainStep((struct GostData*)&inKey, false, true, inCryptEntity); // Шифруем синхропосылку главным шагом.
		inData->m_lData ^= inKey; //  Накладываем гамму.
		//inData++;
	}
}
// ========================================================================================== //
void GostGammaFeedBackBlockEncode(struct GostData* inData, const unsigned int& iDataSize, ULONG64 inKey, const struct CGost89Crypt* inCryptEntity)
{
	inKey = GetRGPCH(inKey); // Инициализируем РГПСЧ зашифрованной синхропосылкой.

	unsigned int i = 0;
	for (i = 0; i < iDataSize / 8; i++)
	{
		DoMainStep((struct GostData*)&inKey, false, true, inCryptEntity); // Шифруем синхропосылку главным шагом.
		inData->m_lData ^= inKey;
		inKey = inData->m_lData;
		inData++;
	}
}
// ========================================================================================== //
void GostGammaFeedBackBlockDecode(struct GostData* inData, const unsigned int& iDataSize, ULONG64 inKey, const struct CGost89Crypt* inCryptEntity)
{
	inKey = GetRGPCH(inKey); // Инициализируем РГПСЧ зашифрованной синхропосылкой.

	unsigned int i = 0;
	ULONG64 tmp;
	for (i = 0; i < iDataSize / 8; i++)
	{
		tmp = inData->m_lData;
		DoMainStep((struct GostData*)&inKey, false, true, inCryptEntity); // Шифруем синхропосылку главным шагом.
		inData->m_lData ^= inKey;
		inKey = tmp;
		inData++;
	}
}
// ========================================================================================== //
ULONG64 GetMessageAuthCode(struct GostData* inData, const unsigned int iDataSize, ULONG64 inKey, const struct CGost89Crypt* inCryptEntity)
{
	ULONG64 iMAC = inKey;
	DoMainStep((struct GostData*)&iMAC, true, true, inCryptEntity); // Шифруем синхропосылку методом выработки имитовставки.

	unsigned int i = 0;
	for (i = 0; i < iDataSize / 8; i++)
	{
		iMAC ^= inData->m_lData;
		//inData++;
	}
	return iMAC;
}
// ========================================================================================== //
void SetKey(const UINT32* inData, struct CGost89Crypt* inCryptEntity)
{
	memcpy(inCryptEntity->m_uiKey, inData, sizeof(UINT32) * 8);
}
// ========================================================================================== //
void SetTable(const UINT8* inData, struct CGost89Crypt* inCryptEntity)
{
	memcpy(inCryptEntity->m_iTable, inData, sizeof(UINT8) * 128);
}
// ========================================================================================== //

void TestSimpleCryption(struct CGost89Crypt* inCryptEntity)
{
	printf("%s", " SimpleExchangeMethod: \n");

	char array[8] = { 'h','e','l','l','o','!','!','!' };

	printf("%s", " Original: \n");

	unsigned int i;
	for (i = 0; i < 8; i++)
		printf("%c", array[i]);
	printf("%s", "\n");

	DoGenerateKey(inCryptEntity);
	DoGenerateTable(inCryptEntity);

	DoMainStep((GostData*)array, false, true, inCryptEntity);

	printf("%s", " Crypted: \n");

	for (i = 0; i < 8; i++)
		printf("%c", array[i]);
	printf("%s", "\n");

	DoMainStep((GostData*)array, false, false, inCryptEntity);

	printf("%s", " Decrypted: \n");

	for (i = 0; i < 8; i++)
		printf("%c", array[i]);
	printf("%s", "\n");
}
// ========================================================================================== //
void TestGammaCryption(struct CGost89Crypt* inCryptEntity)
{
	printf("%s", " SimpleSimpleGammaMethod: \n");

	DoGenerateKey(inCryptEntity);
	DoGenerateTable(inCryptEntity);

	unsigned int i = 0;
	unsigned int z = 0;
	char array[4][8] = { 'h','e','l','l','o','!','!','!',
		'w','o','r','l','d','!','!','!',
		'h','e','l','l','o','!','!','!',
		'w','o','r','l','d','!','!','!' };

	printf("%s", " Original: \n");

	for (z = 0; z < 4; z++)
	{
		for (i = 0; i < 8; i++)
			printf("%c", array[z][i]);
		printf("%s", "\n");
	}

	unsigned int iKey[2];
	iKey[0] = GetRandom(0, RAND_MAX);
	iKey[1] = GetRandom(0, RAND_MAX);
	GostGammaBlockEncode((GostData*)array, 4, (ULONG64)&iKey[0], inCryptEntity);

	printf("%s", " Crypted: \n");

	for (z = 0; z < 4; z++)
	{
		for (i = 0; i < 8; i++)
			printf("%c", array[z][i]);
		printf("%s", "\n");
	}

	GostGammaBlockEncode((GostData*)array, 4, (ULONG64)&iKey[0], inCryptEntity);

	printf("%s", " Decrypted: \n");

	for (z = 0; z < 4; z++)
	{
		for (i = 0; i < 8; i++)
			printf("%c", array[z][i]);
		printf("%s", "\n");
	}
}
// ========================================================================================== //
void TestGammaFeedBackCryption(struct CGost89Crypt* inCryptEntity)
{
	printf("%s", " SimpleGammaFeedBackMethod: \n");

	DoGenerateKey(inCryptEntity);
	DoGenerateTable(inCryptEntity);

	unsigned int i = 0;
	unsigned int z = 0;
	char array[4][8] = { 'h','e','l','l','o','!','!','!',
		'w','o','r','l','d','!','!','!',
		'h','e','l','l','o','!','!','!',
		'w','o','r','l','d','!','!','!' };

	printf("%s", " Original: \n");

	for (z = 0; z < 4; z++)
	{
		for (i = 0; i < 8; i++)
			printf("%c", array[z][i]);
		printf("%s", "\n");
	}

	unsigned int iKey[2];
	iKey[0] = GetRandom(0, RAND_MAX);
	iKey[1] = GetRandom(0, RAND_MAX);
	GostGammaFeedBackBlockEncode((GostData*)array, 32, (ULONG64)&iKey[0], inCryptEntity);

	printf("%s", " Crypted: \n");

	for (z = 0; z < 4; z++)
	{
		for (i = 0; i < 8; i++)
			printf("%c", array[z][i]);
		printf("%s", "\n");
	}


	GostGammaFeedBackBlockDecode((GostData*)array, 32, (ULONG64)&iKey[0], inCryptEntity);

	printf("%s", " Decrypted: \n");

	for (z = 0; z < 4; z++)
	{
		for (i = 0; i < 8; i++)
			printf("%c", array[z][i]);
		printf("%s", "\n");
	}
}
// ========================================================================================== //
void TestMessageAuthCode(struct CGost89Crypt* inCryptEntity)
{
	printf("%s", " TestMessageAuthCode: \n");

	char array[4][8] = { 'h','e','l','l','o','!','!','!',
		'w','o','r','l','d','!','!','!',
		'h','e','l','l','o','!','!','!',
		'w','o','r','l','d','!','!','!' };

	DoGenerateKey(inCryptEntity);
	DoGenerateTable(inCryptEntity);

	printf("%s", " One Step: \n");
	unsigned int iKey[2];
	iKey[0] = GetRandom(0, RAND_MAX);
	iKey[1] = GetRandom(0, RAND_MAX);
	ULONG64 iOneStepMac = GetMessageAuthCode((GostData*)array, 4, (ULONG64)&iKey[0], inCryptEntity);

	printf("%s%d\n", "One Step Mac = ", (int)iOneStepMac);

	printf("%s", " Second Step: \n");
	ULONG64 iSecondStepMac = GetMessageAuthCode((GostData*)array, 4, (ULONG64)&iKey[0], inCryptEntity);

	printf("%s%d\n", "Second Step Mac = ", (int)iSecondStepMac);

	if (iOneStepMac == iSecondStepMac)
		printf("%s\n", "First Test Done!");
	else
		printf("%s\n", "First Test Failed!");

	array[0][0] = 'w';
	ULONG64 iThirdStepMac = GetMessageAuthCode((GostData*)array, 4, (ULONG64)&iKey[0], inCryptEntity);

	if (iThirdStepMac != iSecondStepMac)
		printf("%s\n", "Second Test Done!");
	else
		printf("%s\n", "Second Test Failed!");

}
// ========================================================================================== //
void TestAll()
{
	CGost89Crypt ctx;
	TestSimpleCryption(&ctx);
	TestGammaCryption(&ctx);
	TestGammaFeedBackCryption(&ctx);
	TestMessageAuthCode(&ctx);
}
// ========================================================================================== //

#define KEY_SIZE 512

extern "C"
{
	int  get_combination(int cards[], int cards_count)
	{
		std::multiset<Card> tmp;
		Combination cmb;

		int offset = 0;
		for (int i = 0; i < cards_count; i++)
		{
			Card c;
			c.suit = (int)cards[offset];
			c.value = (int)cards[offset + 1];
			offset += 2;

			tmp.insert(c);
		}

		return getCombination(tmp, cmb);
	}

	int  test()
	{
		return 666;
	}

	void shuffle(int arr[], int arr_size)
	{
		std::vector<int> v;
		for (int i = 0; i < arr_size; i++)
			v.push_back(arr[i]);

		std::array<unsigned char, KEY_SIZE> seed_data;
		std::random_device rd;
		std::generate_n(seed_data.data(), seed_data.size(), [&]() {return rd(); });
		std::seed_seq seed(std::begin(seed_data), std::end(seed_data));

		std::mt19937 g(seed);

		for (unsigned int i = 0; i < 12; i++)
			std::shuffle(v.begin(), v.end(), g);

		memcpy(&arr[0], &v[0], arr_size * sizeof(int));
	}

	int  get_random_from_range(int from, int to)
	{
		std::array<unsigned char, KEY_SIZE> seed_data;
		std::random_device rd;
		std::generate_n(seed_data.data(), seed_data.size(), [&]() {return rd(); });
		std::seed_seq seed(std::begin(seed_data), std::end(seed_data));

		std::mt19937 g(seed);

		std::uniform_int_distribution<int> uni(from, to);
		return uni(g);
	}

	uint32_t  get_random_from_range_uint(uint32_t from, uint32_t to)
	{
		std::array<unsigned char, KEY_SIZE> seed_data;
		std::random_device rd;
		std::generate_n(seed_data.data(), seed_data.size(), [&]() {return rd(); });
		std::seed_seq seed(std::begin(seed_data), std::end(seed_data));

		std::mt19937 g(seed);

		std::uniform_int_distribution<uint32_t> uni(from, to);
		return uni(g);
	}

	void  generate_random_range(unsigned char data[], int data_size)
	{
		std::vector<unsigned char> out_data;
		out_data.resize(data_size);

		std::array<unsigned char, KEY_SIZE> seed_data;
		std::random_device rd;
		std::generate_n(seed_data.data(), seed_data.size(), [&]() {return rd(); });
		std::seed_seq seed(std::begin(seed_data), std::end(seed_data));
		std::mt19937 g(seed);
		std::generate_n(out_data.data(), out_data.size(), [&]() {return g(); });

		memcpy(&data[0], &out_data[0], out_data.size());
	}

	void  crypt_data(unsigned char data[], unsigned char mac[],  int data_len,  unsigned int key[],  unsigned int synchro[])
	{
		CGost89Crypt g_ctx;
		memset(&g_ctx, 0, sizeof(CGost89Crypt));
		memcpy(&g_ctx.m_uiKey, &key, sizeof(uint32_t) * 8);
		memcpy(&g_ctx.m_iTable, &m_iTable, 128);

		ULONG64 sync_tmp;
		memcpy(&sync_tmp, &synchro[0], 8);

		//ULONG64 m = GetMessageAuthCode((GostData*)data, 8, sync_tmp, &g_ctx);
		//memcpy( &mac[0], &m, 8);

		GostGammaBlockEncode((GostData*)data, 1, sync_tmp, &g_ctx);
	}

	void  decrypt_data(unsigned char data[], unsigned char mac[],  int data_len,  unsigned int key[], unsigned int synchro[])
	{
		CGost89Crypt g_ctx;
		memset(&g_ctx, 0, sizeof(CGost89Crypt));
		memcpy(&g_ctx.m_uiKey, &key, sizeof(uint32_t)*8);
		memcpy(&g_ctx.m_iTable, &m_iTable, 128);

		ULONG64 sync_tmp;
		memcpy(&sync_tmp, &synchro[0], 8);
		GostGammaBlockEncode((GostData*)data, 1, sync_tmp, &g_ctx);

		//ULONG64 m = GetMessageAuthCode((GostData*)data, 8, sync_tmp, &g_ctx);
		//memcpy(&mac[0], &m, 8);
	}

	unsigned char* crypt_data1(unsigned char data[], unsigned int key[], unsigned int synchro[])
	{
		CGost89Crypt g_ctx;
		memset(&g_ctx, 0, sizeof(CGost89Crypt));
		memcpy(&g_ctx.m_uiKey, &key[0], 32);
		memcpy(&g_ctx.m_iTable, &m_iTable, 128);

		ULONG64 sync_tmp;
		memcpy(&sync_tmp, &synchro[0], 8);

		unsigned char* tmp = new unsigned char[8];

		memcpy(tmp, data, 8);
		GostGammaBlockEncode((GostData*)tmp, 1, sync_tmp, &g_ctx);
		return tmp;
	}

	unsigned char* decrypt_data1(unsigned char data[], unsigned int key[], unsigned int synchro[])
	{
		CGost89Crypt g_ctx;
		memset(&g_ctx, 0, sizeof(CGost89Crypt));
		memcpy(&g_ctx.m_uiKey, &key[0], 32);
		memcpy(&g_ctx.m_iTable, &m_iTable, 128);

		ULONG64 sync_tmp;
		memcpy(&sync_tmp, &synchro[0], 8);

		unsigned char* tmp = new unsigned char[8];
		memcpy(tmp, data, 8);
		GostGammaBlockEncode((GostData*)tmp, 1, sync_tmp, &g_ctx);
		return tmp;
	}

	unsigned char* get_mac1(unsigned char data[], unsigned int key[], unsigned int synchro[])
	{
		CGost89Crypt g_ctx;
		memset(&g_ctx, 0, sizeof(CGost89Crypt));
		memcpy(&g_ctx.m_uiKey, &key[0], 32);
		memcpy(&g_ctx.m_iTable, &m_iTable, 128);

		ULONG64 sync_tmp;
		memcpy(&sync_tmp, &synchro[0], 8);

		unsigned char* tmp = new unsigned char[8];
	    ULONG64 m = GetMessageAuthCode((GostData*)data, 8, sync_tmp, &g_ctx);
		memcpy(tmp, &m, 8);
		return tmp;
	}

	int freeMem(char* arrayPtr) {
		delete[] arrayPtr;
		return 0;
	}
}



