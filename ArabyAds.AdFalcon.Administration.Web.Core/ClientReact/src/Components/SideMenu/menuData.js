export default {
    items: [
        {
            id: '1',
            label: 'Dashboard',
            icon: '',
            href: 'Dashboard',
            class: '',
            items: [
                {
                    id: '1-1',
                    label: 'Advertisors',
                    icon: '',
                    items: [
                        {
                            id: '1-1-1',
                            label: 'Etisalat',
                            icon: '',
                            items: [
                                {
                                    id: '1-1-1-1',
                                    label: 'Campains',
                                    icon: '',
                                    action: (e, item) => {
                                        alert(`Id:${item.id}, Label:${item.label}`);
                                    }
                                },
                                {
                                    id: '1-1-1-2',
                                    label: 'Audiance List',
                                    icon: '',
                                    action: (e, item) => {
                                        alert(`Id:${item.id}, Label:${item.label}`);
                                    }
                                }
                            ],
                            showBranchesLine: true,
                            active: true
                        }
                    ],
                    showBranchesLine: false,
                    active: false
                }
            ],
            showBranchesLine: false,
            active: false
        },
        {
            id: '2',
            label: 'Account Information',
            active: false,
            action: (e, item) => {
                alert(`Id:${item.id}, Label:${item.label}`);
            }
        },
        {
            id: '3',
            label: 'ADM Account Settings',
            active: false,
            action: (e, item) => {
                alert(`Id:${item.id}, Label:${item.label}`);
            }
        },
        {
            id: '4',
            label: 'Account Balance',
            active: false,
            action: (e, item) => {
                alert(`Id:${item.id}, Label:${item.label}`);
            }
        },
        {
            id: '5',
            label: 'General Settings',
            active: false,
            action: (e, item) => {
                alert(`Id:${item.id}, Label:${item.label}`);
            }
        },
        {
            id: '6',
            label: 'Audit Trail',
            active: false,
            action: (e, item) => {
                alert(`Id:${item.id}, Label:${item.label}`);
            }
        },
        {
            id: '7',
            label: 'Transaction History',
            active: false,
            action: (e, item) => {
                alert(`Id:${item.id}, Label:${item.label}`);
            }
        },
        {
            id: '8',
            label: 'Invitations',
            active: false,
            action: (e, item) => {
                alert(`Id:${item.id}, Label:${item.label}`);
            }
        },
        {
            id: '9',
            label: 'User Management',
            active: false,
            action: (e, item) => {
                alert(`Id:${item.id}, Label:${item.label}`);
            }
        }
    ],
    showBranchesLine: false,
    active: true
};