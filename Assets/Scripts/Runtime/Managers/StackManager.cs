using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackManager : MonoBehaviour
{
    //Burada sadece get özelliði olan property oluþturuyoruz.
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

    //const readonly'e göre daha optimize ama yerine göre tercih edilmesi gerekli.
    //constlar baþka bir class bünyesinde barýndýrýlmasý tercih ediliyor.
    private readonly string _stackDataPath = "Data/CD_Stack";
        
    private void Awake()
    {
        _data = GetStackData();
        Init();
    }

    private void Init()
    {
        //Stackler Lerplenerek hareketi burada yazýlacak, yýlan gibi hareket mekaniði diyebiliriz. StackMoverCommand.
        _stackMoverCommand = new StackMoverCommand(ref _data);
        //Listeye yeni bir eleman eklemek için kullanacaðýmýz Command ItemAdderOnStackCommand.
        _itemAdderOnStackCommand = new ItemAdderOnStackCommand(this, ref _collectableStack, ref _data);
        //Listeden eleman çýkarmak için kullanacaðýmýz Command ItemAdderOnStackCommand.
        _itemRemoverOnStackCommand = new ItemRemoverOnStackCommand(this, ref _collectableStack);
        //Animasyonlarýn kontrolü için kullanýlacak Command StackAnimatorCommand
        _stackAnimatorCommand = new StackAnimatorCommand(this, _data, ref _collectableStack);
        //Karaterin topladýðý para veya altýnlarý engele çarptýðýnda zýplatarak yer düþmesini saðlayacak command StackJumperCommand
        _stackJumperCommand = new StackJumperCommand(_data, ref _collectableStack);
        //Oyunun sonunda paralarýn konveyore konup ana atmye aktarýlmasý için yazýlmýþ command
        _stackInteractionWithConveryorCommand = new StackInteractionWithConveyorCommand(this, ref _collectableStack);
        //Para, deðiþim yerinden geçtiðinde önce altýna, sonra elmasa dönüþüyor ve hem mesh olarak hem de deðer olarak deðiþim yaþanýyor
        //Burada o iþlemler yapýlacak.
        _stackTypeUpdaterCommand = new StackTypeUpdaterCommand(ref _collectableStack);
        //Stackin oluþturulmasý için geçerli COmmand.
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
        //GetCurrentValue + 1 dememizin sebebi deðer 0'dan baþlýyor ekleme yapmasý lazým.
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
        //Yerine göre foreach, yerine göre for daha hýzlýdýr. Ama normal þartlarda for daha hýzlýdýr.
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(0).gameObject);
        }

        //Stack ve Quee LIFO ve FIFO mantýðý olduðu için otomatikman trimexcess yapýlýyor, yani listedi obje temizlendiðinde, mesela 10 obje var,
        //10 boþ obje gözükmüyor, 0 obje oluyor, TrimExcess ile.
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
