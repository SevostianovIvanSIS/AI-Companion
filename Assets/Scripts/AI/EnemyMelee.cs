﻿using UnityEngine;
using System.Collections;

public class EnemyMelee : Entity {

    private AIBrain brain;
    private GunController gunController;

    // Use this for initialization
    public override void Start()
    {

        base.Start();

        EntityType type = EntityType.Enemy | EntityType.MeleeEnemy;
        AddType(type);

        enemyNearby = false;
        blackboard = new Blackboard();
        gunController = GetComponent<GunController>();

        blackboard.treeData.Add("entity", this);
        blackboard.treeData.Add("eyes", eyes);
        blackboard.treeData.Add("gunController", gunController);

        NodeSequencer root = new NodeSequencer(new BTNode[]
            {
                new NodeSelector(new BTNode[]
                {
                    new NodeSelector(new BTNode[]
                    {
                        new ActionCheckForEnemiesInSight(),
                        new NodeAlwaysFail(
                            new NodeSequencer(new BTNode[]
                            {
                                new ActionCheckHitLocation(),
                                new ActionRequestPathToTarget()
                            })
                        ),
                        new NodeSequencer(new BTNode[]
                        {
                            new ActionGetLastSightedSearchPosition(),
                            new ActionRequestPathToTarget()
                        }),
                        new NodeSequencer(new BTNode[]
                        {
                            new ActionSearchingForEnemyBool(),
                            new NodeInverter(
                                new ActionReachedTarget()
                            )
                        }),
                        new NodeSequencer(new BTNode[]
                        {
                            new ActionCheckForEnemiesNearby(),
                            new ActionStopMovement()
                        })
                    }),
                    new NodeSequencer(new BTNode[]
                    {
                        new ActionReachedTarget(),
                        new ActionGetWanderLocation(),
                        new NodeAlwaysFail(
                            new ActionRequestPathToTarget()
                        )
                    })
                }),
                new NodeSelector(new BTNode[]
                {
                    new ActionCheckForEnemiesInSight(),
                    new NodeAlwaysFail(
                        new ActionLookForEnemy()
                    )
                }),
                new NodeSelector(new BTNode[]
                {
                    new NodeSequencer(new BTNode[]
                    {
                        new NodeSequencer(new BTNode[]
                        {
                            new NodeAlwaysSuccess(
                                new NodeCounter(
                                    new ActionMoveToPlayer(), 1
                                )
                            ),
                            new ActionUseWeapon(),
                            new ActionReachedTarget()
                        })
                    }),
                }),
                new ActionRequestPathToTarget()
            });

        brain = new AIBrain(root, blackboard);
        brain.Start();
    }

    void Update()
    {
        brain.Update();
    }
}
