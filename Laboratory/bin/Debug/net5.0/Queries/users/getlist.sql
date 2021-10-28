SELECT _tss.id, _tss.subject, sum(parent_data.parent_count + child_data.childs_count) usage_count
FROM /*(My Name)*/ technical_support_subject _tss
    LEFT OUTER JOIN (
    SELECT tss.id, count(ts.technical_support_subject_id) parent_count
    FROM technical_support_subject tss
    LEFT OUTER JOIN technical_support ts /*(Test 2)*/ on tss.id = ts.technical_support_subject_id and ts.is_active = true
    WHERE tss.is_active = true and tss.parent_id = 0
    GROUP BY tss.id
    ) parent_data ON _tss.id = parent_data.id
    LEFT OUTER JOIN (
    SELECT data.parent_id, sum(data.child_count) childs_count FROM (
    SELECT tss.parent_id, count(ts.technical_support_subject_id) /*(Test 3)*/ child_count
    FROM technical_support_subject tss
    LEFT OUTER JOIN technical_support ts on tss.id = ts.technical_support_subject_id
    WHERE ts.is_active = true and tss.parent_id <> 0
    GROUP BY tss.subject, tss.parent_id, tss.id
    ) as data
    group by data.parent_id
    ) child_data ON _tss.id = child_data.parent_id
WHERE _tss.is_active = true AND _tss.parent_id = 0
GROUP BY _tss.id