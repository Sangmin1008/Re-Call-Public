using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Runtime.CompilerServices;
public class ProjectilePool : Singleton<ProjectilePool>
{
    [SerializeField] private ProjectileController[] _projectilePrefabs;
    [SerializeField] private int _poolSize = 10;

    private Dictionary<int, List<ProjectileController>> _projectilePool;

    private void Awake()
    {
        base.Awake();

        _projectilePool = new Dictionary<int, List<ProjectileController>>();
        for(int i = 0; i < _projectilePrefabs.Length; i++)
        {
            _projectilePool[i] = new List<ProjectileController>();
            for(int j = 0; j < _poolSize; j++)
            {
                Debug.Log("ProjectilePool: " + i);
                GameObject projectile = Instantiate(_projectilePrefabs[i].gameObject, transform);
                projectile.SetActive(false);
                _projectilePool[i].Add(projectile.GetComponent<ProjectileController>());
            }
        }
    }

    public ProjectileController GetProjectile(int index, Vector3 position, Quaternion rotation)
    {
        foreach(var projectile in _projectilePool[index])
        {
            if(!projectile.gameObject.activeSelf)
            {
                projectile.transform.position = position;
                projectile.transform.rotation = rotation;
                projectile.gameObject.SetActive(true);
                return projectile;
            }

        }
        Debug.Log("ProjectilePool: " + index + " is full");
        GameObject newProjectile = Instantiate(_projectilePrefabs[index].gameObject, position, rotation);
        _projectilePool[index].Add(newProjectile.GetComponent<ProjectileController>());
        newProjectile.SetActive(true);
        return newProjectile.GetComponent<ProjectileController>();
    }

    public void ReturnProjectile(ProjectileController projectile)
    {
        projectile.gameObject.SetActive(false);
    }
}