using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackManager : MonoBehaviour
{
    //Burada sadece get �zelli�i olan property olu�turuyoruz.
    public StackJumperCommand StackJumperCommand => _stackJumperCommand;
    public StackTypeUpdaterCommand StackTypeUpdaterCommand => _stackTypeUpdaterCommand;
    public ItemAdderOnStackCommand ItemAdderOnStackCommand => _itemAdderOnStackCommand;
    public bool LastCheck
    {
        get => _lastCheck;
        set => _lastCheck = value;
    }

    private StackMoverCommand _stackMoverCommand;
    private ItemAdderOnStackCommand _itemAdderOnStackCommand;
    private ItemRemoverOnStackCommand _itemRemoverOnStackCommand;
    private StackAnimatorCommand _stackAnimatorCommand;
    private StackJumperCommand _stackJumperCommand;
    private StackTypeUpdaterCommand _stackTypeUpdaterCommand;
    private StackInteractionWithConveyorCommand _stackInteractionWithConveryorCommand;
    private StackInitializerCommand _stackInitializerCommand;

    [SerializeField]
    private GameObject levelHolder;
    [SerializeField]
    private GameObject money;

    private StackData _data;
    private List<GameObject> _collectableStack = new List<GameObject>();
    private bool _lastCheck;

    //const readonly'e g�re daha optimize ama yerine g�re tercih edilmesi gerekli.
    //constlar ba�ka bir class b�nyesinde bar�nd�r�lmas� tercih ediliyor.
    private readonly string _stackDataPath = "Data/CD_Stack";
        
    private void Awake()
    {
        _data = GetStackData();
        Init();
    }

    private void Init()
    {
        //Stackler Lerplenerek hareketi burada yaz�lacak, y�lan gibi hareket mekani�i diyebiliriz. StackMoverCommand.
        _stackMoverCommand = new StackMoverCommand(ref _data);
        //Listeye yeni bir eleman eklemek i�in kullanaca��m�z Command ItemAdderOnStackCommand.
        _itemAdderOnStackCommand = new ItemAdderOnStackCommand(this, ref _collectableStack, ref _data);
        //Listeden eleman ��karmak i�in kullanaca��m�z Command ItemAdderOnStackCommand.
        _itemRemoverOnStackCommand = new ItemRemoverOnStackCommand(this, ref _collectableStack);
        //Animasyonlar�n kontrol� i�in kullan�lacak Command StackAnimatorCommand
        _stackAnimatorCommand = new StackAnimatorCommand(this, _data, ref _collectableStack);
        //Karaterin toplad��� para veya alt�nlar� engele �arpt���nda z�platarak yer d��mesini sa�layacak command StackJumperCommand
        _stackJumperCommand = new StackJumperCommand(_data, ref _collectableStack);
        //Oyunun sonunda paralar�n konveyore konup ana atmye aktar�lmas� i�in yaz�lm�� command
        _stackInteractionWithConveryorCommand = new StackInteractionWithConveyorCommand(this, ref _collectableStack);
        //Para, de�i�im yerinden ge�ti�inde �nce alt�na, sonra elmasa d�n���yor ve hem mesh olarak hem de de�er olarak de�i�im ya�an�yor
        //Burada o i�lemler yap�lacak.
        _stackTypeUpdaterCommand = new StackTypeUpdaterCommand(ref _collectableStack);
        //Stackin olu�turulmas� i�in ge�erli COmmand.
        _stackInitializerCommand = new StackInitializerCommand(this, ref money);
    }

    private StackData GetStackData()
    {
        return Resources.Load<CD_Stack>(_stackDataPath).stackData;
    }

    private void OnEnable()
    {
        SubscribeEvents();
    }

    private void SubscribeEvents()
    {
        StackSignals.Instance.onInteractionCollectable += OnInteractionWithCollectable;
        StackSignals.Instance.onInteractionObstacle += _itemRemoverOnStackCommand.Execute;
        StackSignals.Instance.onInteractionATM += OnInteractionWithATM;
        StackSignals.Instance.onInteractionConveyor += _stackInteractionWithConveryorCommand.Execute;
        StackSignals.Instance.onStackFollowPlayer += OnStackMove;
        StackSignals.Instance.onUpdateType += _stackTypeUpdaterCommand.Execute;
        CoreGameSignals.Instance.onPlay += OnPlay;
        CoreGameSignals.Instance.onReset += OnReset;
    }

    private void OnStackMove(Vector2 direction)
    {
        transform.position = new Vector3(0, gameObject.transform.position.y, direction.y + 2f);
        if (gameObject.transform.childCount > 0)
        {
            _stackMoverCommand.Execute(direction.x, _collectableStack);
        }
    }

    private void OnInteractionWithATM(GameObject collectableGameObject)
    {
        //GetCurrentValue + 1 dememizin sebebi de�er 0'dan ba�l�yor ekleme yapmas� laz�m.
        ScoreSignals.Instance.onSetAtmScore((int)collectableGameObject.GetComponent<CollectableManager>().GetCurrentValue() + 1);
        if (_lastCheck == false)
        {
            _itemRemoverOnStackCommand.Execute(collectableGameObject);
        }
        else
        {
            collectableGameObject.SetActive(false);
        }
    }

    private void OnInteractionWithCollectable(GameObject collectableGameObject)
    {
        DOTween.Complete(_stackJumperCommand);
        _itemAdderOnStackCommand.Execute(collectableGameObject);
        StartCoroutine(_stackAnimatorCommand.Execute());
        _stackTypeUpdaterCommand.Execute();
    }

    private void OnPlay()
    {
        _stackInitializerCommand.Execute();
    }

    private void OnReset()
    {
        _lastCheck = false;
        //Yerine g�re foreach, yerine g�re for daha h�zl�d�r. Ama normal �artlarda for daha h�zl�d�r.
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(0).gameObject);
        }

        //Stack ve Quee LIFO ve FIFO mant��� oldu�u i�in otomatikman trimexcess yap�l�yor, yani listedi obje temizlendi�inde, mesela 10 obje var,
        //10 bo� obje g�z�km�yor, 0 obje oluyor, TrimExcess ile.
        _collectableStack.Clear();
        _collectableStack.TrimExcess();
    }

    private void UnSubscribeEvents()
    {
        StackSignals.Instance.onInteractionCollectable -= OnInteractionWithCollectable;
        StackSignals.Instance.onInteractionObstacle -= _itemRemoverOnStackCommand.Execute;
        StackSignals.Instance.onInteractionATM -= OnInteractionWithATM;
        StackSignals.Instance.onInteractionConveyor -= _stackInteractionWithConveryorCommand.Execute;
        StackSignals.Instance.onStackFollowPlayer += OnStackMove;
        StackSignals.Instance.onUpdateType += _stackTypeUpdaterCommand.Execute;
        CoreGameSignals.Instance.onPlay -= OnPlay;
        CoreGameSignals.Instance.onReset -= OnReset;
    }

    private void OnDisable()
    {
        UnSubscribeEvents();
    }
}
